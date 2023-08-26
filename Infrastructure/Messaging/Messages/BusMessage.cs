namespace Microservice.Email.Infrastructure.Messaging.Messages;

public sealed class BusMessage<T>
{
    public required string Exchange { get; init; }
    public required T? Payload { get; init; }
}
