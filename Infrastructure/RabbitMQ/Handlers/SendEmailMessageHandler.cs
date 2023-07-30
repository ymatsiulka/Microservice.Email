using Microservice.Email.Core.Contracts.Requests;
using Microservice.Email.Core.Services.Interfaces;
using Microservice.Email.Infrastructure.RabbitMQ.Interfaces;

namespace Microservice.Email.Infrastructure.RabbitMQ.Handlers;

public sealed class SendEmailMessageHandler : IRabbitMQMessageHandler<SendEmailRequest>
{
    private readonly ILogger<SendEmailMessageHandler> logger;
    private readonly IEmailService emailService;

    public SendEmailMessageHandler(ILogger<SendEmailMessageHandler> logger,
        IEmailService emailService)
    {
        this.logger = logger;
        this.emailService = emailService;
    }

    public async Task Handle(SendEmailRequest payload)
    {
        if (payload == null)
            throw new ArgumentNullException(nameof(payload));
        
        logger.Log(LogLevel.Information, $"Starting handling message with subject={payload.Subject}");

        await emailService.Send(payload);

        logger.Log(LogLevel.Information, $"Ends handling message with subject={payload.Subject}");
    }
}
