namespace Microservice.Email.Infrastructure.Messaging.Settings;

public sealed class QueueProperties
{
    public required int RetriesCount { get; set; }
    public required long MessageLifeTime { get; set; }
    public required bool AddDeadLetterExchange { get; set; }
    public required int ConsumersCount { get; set; }
}
