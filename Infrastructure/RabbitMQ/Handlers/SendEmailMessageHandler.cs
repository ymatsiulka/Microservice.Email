using Microservice.Email.Infrastructure.RabbitMQ.Interfaces;
using RabbitMQ.Client.Events;

namespace Microservice.Email.Infrastructure.RabbitMQ.Handlers;

public sealed class SendEmailMessageHandler : IRabbitMQMessageHandler
{
    private readonly ILogger<SendEmailMessageHandler> logger;

    public SendEmailMessageHandler(ILogger<SendEmailMessageHandler> logger)
    {
        this.logger = logger;
    }

    public Task Handle(BasicDeliverEventArgs args)
    {
        if (args == null)
            throw new ArgumentNullException(nameof(args));

        logger.Log(LogLevel.Information, "email sent");
        return Task.CompletedTask;
    }
}
