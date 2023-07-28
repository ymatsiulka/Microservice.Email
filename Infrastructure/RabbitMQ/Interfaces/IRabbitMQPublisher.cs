namespace Microservice.Email.Infrastructure.RabbitMQ.Interfaces;

public interface IRabbitMQPublisher
{
    Task Publish<T>(RabbitMQMessage<T> message);
}
