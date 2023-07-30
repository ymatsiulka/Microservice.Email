using Microservice.Email.Extensions;
using Microservice.Email.Infrastructure.RabbitMQ.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Consumer = RabbitMQ.Client.Events.AsyncEventingBasicConsumer;
using Handler = RabbitMQ.Client.Events.AsyncEventHandler<RabbitMQ.Client.Events.BasicDeliverEventArgs>;

namespace Microservice.Email.Infrastructure.RabbitMQ;

public class RabbitMQBusInitializer : IHostedService, IDisposable
{
    private readonly BusSettings busSettings;
    private readonly IServiceScope serviceScope;
    private readonly ILogger<RabbitMQBusInitializer> logger;

    private readonly Dictionary<Consumer, Handler> handlers = new();

    public RabbitMQBusInitializer(
        BusSettings busSettings,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<RabbitMQBusInitializer> logger)
    {
        this.busSettings = busSettings;
        this.logger = logger;
        serviceScope = serviceScopeFactory.CreateScope();
    }


    public Task StartAsync(CancellationToken cancellationToken)
    {
        var serviceProvider = serviceScope.ServiceProvider;
        var channel = serviceProvider.GetRequiredService<IRabbitMQChannelService>().GetChannel();
        var exchangeSettings = busSettings.Exchanges;

        foreach (var exchange in exchangeSettings)
        {
            channel.ExchangeDeclare(exchange.Name, ExchangeType.Fanout);
            logger.LogInformation("Exchange declared. Name: {Name}", exchange.Name);

            foreach (var queue in exchange.Queues)
            {
                channel.QueueDeclare(queue.Name, exclusive: false, autoDelete: false);
                channel.QueueBind(queue.Name, exchange.Name, queue.Name);
                logger.LogInformation("Queue declared. Name: {Name}", queue.Name);
                // Get the generic type parameter you want to find the implementation for (e.g., Payload)

                // Find the implementation of IInterface<T> for the specified generic type
                var messageHandler = serviceProvider
                    .GetRequiredService(queue.HandlerType);

                var consumer = new Consumer(channel);
                Handler handler = async (_, args) =>
                {

                    dynamic message = JsonConvert.DeserializeObject(args.Body.FromBytes(),type: queue.PayloadType);
                    // If you want to call the method SomeMethod on the implementation, you can do it like this:
                    var someMethod = messageHandler.GetType().GetMethod("Handle");
                    someMethod.Invoke(messageHandler, new object[] { message });
                };
                consumer.Received += handler;


                handlers.Add(consumer, handler);

                channel.BasicConsume(queue.Name, autoAck: true, consumer: consumer);
                logger.LogInformation("Handler of type: {HandlerType} attached to queue", queue.HandlerType);
            }
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (var (consumer, handler) in handlers)
            consumer.Received -= handler;

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        serviceScope.Dispose();
    }
}
