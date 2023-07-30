namespace Microservice.Email.Infrastructure.RabbitMQ.Interfaces;

public interface IRabbitMQMessageHandler<TPayload> where TPayload : class
{
    Task Handle(TPayload payload);
}
