using Microservice.Email.Infrastructure.Messaging.Settings;
using RabbitMQ.Client;

namespace Microservice.Email.Infrastructure.Messaging.Factories.Interfaces;

public interface IQueueFactory
{
    void CreateQueue(IModel channel, string exchange, QueueSettings queueSettings);
}