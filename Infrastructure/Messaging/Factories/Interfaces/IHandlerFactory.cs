using Microservice.Email.Infrastructure.Messaging.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Microservice.Email.Infrastructure.Messaging.Factories.Interfaces;

public interface IHandlerFactory
{
    AsyncEventHandler<BasicDeliverEventArgs> CreateHandler(IModel channel, QueueSettings queueSettings);
}