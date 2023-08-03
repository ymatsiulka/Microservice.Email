namespace Microservice.Email.Infrastructure.Messaging.Settings;

public class BusSettings
{
    public List<ExchangeSettings> Exchanges { get; } = new();
}