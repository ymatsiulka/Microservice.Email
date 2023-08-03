using ArchitectProg.FunctionalExtensions.Extensions;
using Microservice.Email.Infrastructure.Messaging.Interfaces;
using Microservice.Email.Infrastructure.Messaging.Settings;

namespace Microservice.Email.Infrastructure.Messaging;

public sealed class MessageBusSettingsBuilder : IMessageBusSettingsBuilder
{
    private ExchangeSettings? currentExchange;
    private readonly BusSettings busSettings = new();

    public IMessageBusSettingsBuilder RegisterExchange(string exchangeName)
    {
        if (exchangeName.IsNullOrWhiteSpace())
            throw new ArgumentNullException(nameof(exchangeName));

        var exchangeSettings = new ExchangeSettings
        {
            Name = exchangeName,
            Queues = new List<QueueSettings>()
        };

        busSettings.Exchanges.Add(exchangeSettings);
        currentExchange = exchangeSettings;

        return this;
    }

    public IMessageBusSettingsBuilder RegisterHandler<THandler, TImplementation>(
        string queueName,
        Action<QueueProperties>? configureProperties = null)
        where THandler : IMessageHandler
        where TImplementation : IMessageHandler
    {
        if (queueName.IsNullOrWhiteSpace())
            throw new ArgumentNullException(nameof(queueName));

        if (currentExchange is null)
        {
            var error = "Can't add queue to null exchange. Use MessageBusBuilder.RegisterExchange method first";
            throw new InvalidCastException(error);
        }

        var properties = GetDefaultProperties();
        configureProperties?.Invoke(properties);

        var queueSettings = new QueueSettings
        {
            Name = queueName,
            HandlerType = typeof(THandler),
            HandlerImplementationType = typeof(TImplementation),
            Properties = properties
        };

        currentExchange.Queues.Add(queueSettings);
        return this;
    }

    public BusSettings Build() => busSettings;

    private QueueProperties GetDefaultProperties()
    {
        var result = new QueueProperties
        {
            RetriesCount = 5,
            MessageLifeTime = 600000,
            AddDeadLetterExchange = false,
            ConsumersCount = 10
        };

        return result;
    }
}