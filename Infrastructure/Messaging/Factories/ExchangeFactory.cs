using Microservice.Email.Infrastructure.Messaging.Factories.Interfaces;
using Microservice.Email.Infrastructure.Messaging.Settings;
using RabbitMQ.Client;

namespace Microservice.Email.Infrastructure.Messaging.Factories;

public sealed class ExchangeFactory : IExchangeFactory
{
    private readonly ILogger<ExchangeFactory> logger;

    public ExchangeFactory(ILogger<ExchangeFactory> logger)
    {
        this.logger = logger;
    }

    public async Task CreateExchangeAsync(IChannel channel, ExchangeSettings exchangeSettings)
    {
        await channel.ExchangeDeclareAsync(exchangeSettings.Name, ExchangeType.Fanout);
        logger.LogInformation("Exchange declared. Name: {Name}", exchangeSettings.Name);
    }
}