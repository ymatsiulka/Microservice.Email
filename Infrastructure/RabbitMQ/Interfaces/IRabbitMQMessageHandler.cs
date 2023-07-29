using RabbitMQ.Client.Events;

namespace Microservice.Email.Infrastructure.RabbitMQ.Interfaces;

public interface IRabbitMQMessageHandler
{
    Task Handle(BasicDeliverEventArgs args);
}
