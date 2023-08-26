namespace Microservice.Email.Infrastructure.Messaging.Settings;

public sealed class MessagingSettings
{
    public required string Host { get; init; }
    public required int Port { get; init; }
    public string? Username { get; init; }
    public string? Password { get; init; }
}
