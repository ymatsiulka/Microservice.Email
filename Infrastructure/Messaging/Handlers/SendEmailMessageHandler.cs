using ArchitectProg.FunctionalExtensions.Services.Interfaces;
using Microservice.Email.Core.Contracts.Requests;
using Microservice.Email.Core.Services.Interfaces;

namespace Microservice.Email.Infrastructure.Messaging.Handlers;

public sealed class SendEmailMessageHandler : BaseMessageHandler<SendEmailRequest>
{
    private readonly IEmailService emailService;

    public SendEmailMessageHandler(
        ILogger<SendEmailMessageHandler> logger,
        IEmailService emailService, IJsonSerializer jsonSerializer) : base(logger, jsonSerializer)
    {
        this.emailService = emailService;
    }

    public override async Task Handle(SendEmailRequest payload)
    {
        await emailService.Send(payload);
    }
}