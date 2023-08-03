using Microservice.Email.Infrastructure.Messaging.Factories.Interfaces;
using Microservice.Email.Infrastructure.Messaging.Settings;
using RabbitMQ.Client;

namespace Microservice.Email.Infrastructure.Messaging.Factories;

public class ExchangeFactory : IExchangeFactory
{
    private readonly ILogger<ExchangeFactory> logger;

    public ExchangeFactory(ILogger<ExchangeFactory> logger)
    {
        this.logger = logger;
    }

    public void CreateExchange(IModel channel, ExchangeSettings exchangeSettings)
    {
        channel.ExchangeDeclare(exchangeSettings.Name, ExchangeType.Fanout);
        logger.LogInformation("Exchange declared. Name: {Name}", exchangeSettings.Name);
    }
}