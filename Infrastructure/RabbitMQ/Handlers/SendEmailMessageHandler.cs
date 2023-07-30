using Microservice.Email.Core.Contracts.Requests;
using Microservice.Email.Infrastructure.RabbitMQ.Interfaces;

namespace Microservice.Email.Infrastructure.RabbitMQ.Handlers;

public sealed class SendEmailMessageHandler : IRabbitMQMessageHandler<SendEmailRequest>
{
    private readonly ILogger<SendEmailMessageHandler> logger;

    public SendEmailMessageHandler(ILogger<SendEmailMessageHandler> logger)
    {
        this.logger = logger;
    }

    public Task Handle(SendEmailRequest payload)
    {
        if (payload == null)
            throw new ArgumentNullException(nameof(payload));
        

        logger.Log(LogLevel.Information, $"Starting handling message with subject={payload.Subject}");
        logger.Log(LogLevel.Information, $"Ends handling message with subject={payload.Subject}");
        return Task.CompletedTask;
    }
}
