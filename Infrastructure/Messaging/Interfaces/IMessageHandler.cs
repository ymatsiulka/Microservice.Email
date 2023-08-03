using RabbitMQ.Client.Events;

namespace Microservice.Email.Infrastructure.Messaging.Interfaces;

public interface IMessageHandler
{
    Task Handle(BasicDeliverEventArgs args);
}

public interface IMessageHandler<in T> : IMessageHandler where T : class
{
    Task Handle(T payload);
}