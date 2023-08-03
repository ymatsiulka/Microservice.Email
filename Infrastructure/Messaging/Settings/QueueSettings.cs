namespace Microservice.Email.Infrastructure.Messaging.Settings;

public sealed class QueueSettings
{
    public required string Name { get; init; }
    public required Type HandlerType { get; init; }
    public required Type HandlerImplementationType { get; init; }
    public required QueueProperties Properties { get; init; }
}
