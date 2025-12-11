namespace Microservice.Email.Infrastructure.Messaging.Settings;

public sealed class MessagingSettings
{
    public required string Host { get; init; }
    public required int Port { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
}
