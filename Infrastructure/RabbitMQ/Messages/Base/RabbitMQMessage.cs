namespace Microservice.Email.Infrastructure.RabbitMQ.Messages.Base;

public class RabbitMQMessage<T>
{
    public required string Queue { get; init; }
    public required string Exchange { get; init; }
    public required T? Payload { get; init; }
}
