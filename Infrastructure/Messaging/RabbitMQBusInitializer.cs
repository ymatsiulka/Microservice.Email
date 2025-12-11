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

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var services = serviceScope.ServiceProvider;
        var channelProvider = services.GetRequiredService<IChannelProvider>();

        var channel = await channelProvider.CreateChannelAsync();
        var exchangeSettings = busSettings.Exchanges;

        foreach (var exchange in exchangeSettings)
        {
            await exchangeFactory.CreateExchangeAsync(channel, exchange);

            foreach (var queue in exchange.Queues)
            {
                await queueFactory.CreateQueueAsync(channel, exchange.Name, queue);
                await channel.QueueBindAsync(queue.Name, exchange.Name, queue.Name);

                for (var i = 0; i < queue.Properties.ConsumersCount; i++)
                {
                    var handler = handlerFactory.CreateHandler(channel, queue);

                    var consumer = new Consumer(channel);
                    consumer.ReceivedAsync += handler;

                    handlers.Add(consumer, handler);
                    await channel.BasicConsumeAsync(queue.Name, autoAck: false, consumer: consumer);
                    channel = await channelProvider.CreateChannelAsync();
                }

                logger.LogInformation("Handler of type: {HandlerType} attached to queue", queue.HandlerType);
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (var (consumer, handler) in handlers)
            consumer.ReceivedAsync -= handler;

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        serviceScope.Dispose();
    }
}