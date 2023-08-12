using ArchitectProg.FunctionalExtensions.Extensions;
using ArchitectProg.FunctionalExtensions.Services.Interfaces;
using Microservice.Email.Infrastructure.Messaging.Interfaces;
using RabbitMQ.Client.Events;

namespace Microservice.Email.Infrastructure.Messaging.Handlers;

public abstract class BaseMessageHandler<T> : IMessageHandler<T> where T : class
{
    private readonly ILogger<BaseMessageHandler<T>> logger;
    private readonly IJsonSerializer jsonSerializer;

    protected BaseMessageHandler(ILogger<BaseMessageHandler<T>> logger, IJsonSerializer jsonSerializer)
    {
        this.logger = logger;
        this.jsonSerializer = jsonSerializer;
    }

    public async Task Handle(BasicDeliverEventArgs args)
    {
        logger.LogInformation("Started handle event of type {EventType}, in MessageHandler: {HandlerType}",
            typeof(T).Name,
            GetType().Name);

        var payload = jsonSerializer.Deserialize<T>(args.Body.FromBytes());
        if (payload is null)
        {
            var error = $"MessageHandler of type {GetType().Name} should have payload with type {typeof(T).Name}";
            throw new InvalidCastException(error);
        }

        await Handle(payload);

        logger.LogInformation("Finished handle event of type {EventType}, in MessageHandler: {HandlerType}",
            typeof(T).Name,
            GetType().Name);
    }

    public abstract Task Handle(T payload);
}