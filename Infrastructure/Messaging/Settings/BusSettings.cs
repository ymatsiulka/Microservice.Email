namespace Microservice.Email.Infrastructure.Messaging.Settings;

public sealed class BusSettings
{
    public List<ExchangeSettings> Exchanges { get; } = new();
}