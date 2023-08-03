using Microservice.Email.Infrastructure.Messaging.Factories.Interfaces;
using Microservice.Email.Infrastructure.Messaging.Interfaces;
using Microservice.Email.Infrastructure.Messaging.Settings;
using RabbitMQ.Client;
using Consumer = RabbitMQ.Client.Events.AsyncEventingBasicConsumer;
using Handler = RabbitMQ.Client.Events.AsyncEventHandler<RabbitMQ.Client.Events.BasicDeliverEventArgs>;

namespace Microservice.Email.Infrastructure.Messaging;

public sealed class RabbitMQBusInitializer : IHostedService, IDisposable
{
    private readonly BusSettings busSettings;
    private readonly IExchangeFactory exchangeFactory;
    private readonly IHandlerFactory handlerFactory;
    private readonly IQueueFactory queueFactory;
    private readonly IServiceScope serviceScope;
    private readonly ILogger<RabbitMQBusInitializer> logger;

    private readonly Dictionary<Consumer, Handler> handlers = new();

    public RabbitMQBusInitializer(
        BusSettings busSettings,
        IExchangeFactory exchangeFactory,
        IHandlerFactory handlerFactory,
        IQueueFactory queueFactory,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<RabbitMQBusInitializer> logger)
    {
        this.logger = logger;
        this.busSettings = busSettings;
        this.exchangeFactory = exchangeFactory;
        this.handlerFactory = handlerFactory;
        this.queueFactory = queueFactory;

        serviceScope = serviceScopeFactory.CreateScope();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var services = serviceScope.ServiceProvider;
        var channelProvider = services.GetRequiredService<IChannelProvider>();

        var channel = channelProvider.Channel;
        var exchangeSettings = busSettings.Exchanges;

        foreach (var exchange in exchangeSettings)
        {
            exchangeFactory.CreateExchange(channel, exchange);

            foreach (var queue in exchange.Queues)
            {
                queueFactory.CreateQueue(channel, exchange.Name, queue);
                channel.QueueBind(queue.Name, exchange.Name, queue.Name);

                for (var i = 0; i < queue.Properties.ConsumersCount; i++)
                {
                    var handler = handlerFactory.CreateHandler(channel, queue);

                    var consumer = new Consumer(channel);
                    consumer.Received += handler;

                    handlers.Add(consumer, handler);
                    channel.BasicConsume(queue.Name, autoAck: false, consumer: consumer);
                    channel = channelProvider.CreateChannel();
                }

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