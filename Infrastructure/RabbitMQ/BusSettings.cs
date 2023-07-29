namespace Microservice.Email.Infrastructure.RabbitMQ;

public class BusSettings
{
    private ExchangeSettings? currentExchange;
    private readonly List<ExchangeSettings> exchanges = new();

    public ExchangeSettings? CurrentExchange => currentExchange;
    public IReadOnlyCollection<ExchangeSettings> Exchanges => exchanges;

    public void AddExchange(ExchangeSettings exchange)
    {
        exchanges.Add(exchange);
        currentExchange = exchange;
    }
}
