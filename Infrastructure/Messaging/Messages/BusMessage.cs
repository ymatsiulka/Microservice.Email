namespace Microservice.Email.Infrastructure.Messaging.Messages;

public class BusMessage<T>
{
    public required string Exchange { get; init; }
    public required T? Payload { get; init; }
}
