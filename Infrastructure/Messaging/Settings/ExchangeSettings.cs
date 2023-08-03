namespace Microservice.Email.Infrastructure.Messaging.Settings;

public sealed class ExchangeSettings
{
    public required string Name { get; init; }
    public required List<QueueSettings> Queues { get; init; }
}
