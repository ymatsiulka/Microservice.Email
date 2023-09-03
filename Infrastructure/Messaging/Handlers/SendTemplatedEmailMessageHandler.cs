using ArchitectProg.FunctionalExtensions.Services.Interfaces;
using Microservice.Email.Core.Contracts.Requests;
using Microservice.Email.Core.Services.Interfaces;

namespace Microservice.Email.Infrastructure.Messaging.Handlers;

public sealed class SendTemplatedEmailMessageHandler : BaseMessageHandler<AttachmentsWrapper<SendTemplatedEmailRequest>>
{
    private readonly IEmailService emailService;

    public SendTemplatedEmailMessageHandler(
        ILogger<SendTemplatedEmailMessageHandler> logger,
        IEmailService emailService,
        IJsonSerializer jsonSerializer) : base(logger, jsonSerializer)
    {
        this.emailService = emailService;
    }

    public override async Task Handle(AttachmentsWrapper<SendTemplatedEmailRequest> payload)
    {
        await emailService.SendTemplated(payload);
    }
}