using Microservice.Email.Infrastructure.Messaging.Factories.Interfaces;
using Microservice.Email.Infrastructure.Messaging.Interfaces;
using Microservice.Email.Infrastructure.Messaging.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Microservice.Email.Infrastructure.Messaging.Factories;

public class HandlerFactory : IHandlerFactory
{
    private readonly IServiceScopeFactory serviceScopeFactory;
    private readonly ILogger<HandlerFactory> logger;

    public HandlerFactory(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<HandlerFactory> logger)
    {
        this.serviceScopeFactory = serviceScopeFactory;
        this.logger = logger;
    }

    public AsyncEventHandler<BasicDeliverEventArgs> CreateHandler(IModel channel, QueueSettings queueSettings)
    {
        return async (_, args) =>
        {
            try
            {
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    if (services.GetRequiredService(queueSettings.HandlerType) is not IMessageHandler handler)
                    {
                        var message = $"Can't resolve handler of type {queueSettings.HandlerType}.";
                        throw new InvalidOperationException(message);
                    }

                    await handler.Handle(args);
                    channel.BasicAck(args.DeliveryTag, false);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Message: {Message}. StackTrace: {StackTrace}", ex.Message, ex.StackTrace);
                var retriesCount = GetRetryCount(args.BasicProperties);
                var queueRetries = queueSettings.Properties.RetriesCount;
                channel.BasicReject(args.DeliveryTag, requeue: retriesCount < queueRetries);
            }
        };
    }

    private static int GetRetryCount(IBasicProperties messageProperties)
    {
        var count = 0;
        var headers = messageProperties.Headers;
        if (headers is null)
            return count;

        if (headers.TryGetValue("x-delivery-count", out var header))
        {
            var countAsString = Convert.ToString(header);
            count = Convert.ToInt32(countAsString);
        }

        return count;
    }
}