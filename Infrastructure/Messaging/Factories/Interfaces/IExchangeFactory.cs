using Microservice.Email.Infrastructure.Messaging.Settings;
using RabbitMQ.Client;

namespace Microservice.Email.Infrastructure.Messaging.Factories.Interfaces;

public interface IExchangeFactory
{
    Task CreateExchangeAsync(IChannel channel, ExchangeSettings exchangeSettings);
}