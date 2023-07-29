using Microservice.Email.Core.Contracts.Requests;
using Microservice.Email.Extensions;
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
        var message = args.Body.FromBytes().Deserialize<SendEmailRequest>();
        logger.Log(LogLevel.Information, $"Starting handling message with subject={message.Subject}");
        logger.Log(LogLevel.Information, $"Ends handling message with subject={message.Subject}");
        return Task.CompletedTask;
    }
}
