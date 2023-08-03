using Microservice.Email.Core.Contracts.Requests;
using Microservice.Email.Core.Services.Interfaces;

namespace Microservice.Email.Infrastructure.Messaging.Handlers;

public sealed class SendTemplatedEmailMessageHandler : BaseMessageHandler<SendTemplatedEmailRequest>
{
    private readonly IEmailService emailService;

    public SendTemplatedEmailMessageHandler(
        ILogger<SendTemplatedEmailMessageHandler> logger,
        IEmailService emailService) : base(logger)
    {
        this.emailService = emailService;
    }

    public override async Task Handle(SendTemplatedEmailRequest payload)
    {
        await emailService.SendTemplated(payload);
    }
}