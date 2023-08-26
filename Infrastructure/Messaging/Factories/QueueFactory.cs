using Microservice.Email.Infrastructure.Messaging.Factories.Interfaces;
using Microservice.Email.Infrastructure.Messaging.Settings;
using RabbitMQ.Client;

namespace Microservice.Email.Infrastructure.Messaging.Factories;

public sealed class QueueFactory : IQueueFactory
{
    private const string DeadLetterExchangeFormat = "{0}-dead-letter";

    private readonly ILogger<QueueFactory> logger;
    private readonly IExchangeFactory exchangeFactory;

    public QueueFactory(
        ILogger<QueueFactory> logger,
        IExchangeFactory exchangeFactory)
    {
        this.logger = logger;
        this.exchangeFactory = exchangeFactory;
    }

    public void CreateQueue(IModel channel, string exchange, QueueSettings queueSettings)
    {
        if (queueSettings.Properties.AddDeadLetterExchange)
        {
            var deadLetterExchangeSettings = new ExchangeSettings
            {
                Name = string.Format(DeadLetterExchangeFormat, exchange),
                Queues = new List<QueueSettings>()
            };

            exchangeFactory.CreateExchange(channel, deadLetterExchangeSettings);
        }

        var queueArgs = GetQueueArgs(exchange, queueSettings);
        channel.QueueDeclare(queueSettings.Name, durable: true, exclusive: false, autoDelete: false, queueArgs);
        logger.LogInformation("Queue declared. Name: {Name}", queueSettings.Name);
    }

    private Dictionary<string, object> GetQueueArgs(string exchange, QueueSettings queueSettings)
    {
        var result = new Dictionary<string, object>
        {
            { Headers.XQueueType, "quorum" },
            { Headers.XExpires, queueSettings.Properties.MessageLifeTime }
        };

        if (queueSettings.Properties.AddDeadLetterExchange)
        {
            var deadLetterExchange = string.Format(DeadLetterExchangeFormat, exchange);
            result.Add(Headers.XDeadLetterExchange, deadLetterExchange);
        }

        return result;
    }
}