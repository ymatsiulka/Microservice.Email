using ArchitectProg.FunctionalExtensions.Services.Interfaces;
using Microservice.Email.Core.Contracts.Requests;
using Microservice.Email.Core.Services.Interfaces;

namespace Microservice.Email.Infrastructure.Messaging.Handlers;

public sealed class SendEmailMessageHandler : BaseMessageHandler<AttachmentsWrapper<SendEmailRequest>>
{
    private readonly IEmailService emailService;

    public SendEmailMessageHandler(
        ILogger<SendEmailMessageHandler> logger,
        IEmailService emailService, IJsonSerializer jsonSerializer) : base(logger, jsonSerializer)
    {
        this.emailService = emailService;
    }

    public override async Task Handle(AttachmentsWrapper<SendEmailRequest> payload)
    {
        await emailService.Send(payload);
    }
}