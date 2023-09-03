using FluentEmail.Core;
using Microservice.Email.Core.Factories.Interfaces;
using Microservice.Email.Infrastructure.Smtp.Contracts;
using Microservice.Email.Infrastructure.Smtp.Factories.Interfaces;
using Microservice.Email.Infrastructure.Smtp.Interfaces;

namespace Microservice.Email.Infrastructure.Smtp.Factories;

public sealed class EmailFactory : IEmailFactory
{
    private readonly ISanitizationService sanitizationService;
    private readonly IAttachmentFactory attachmentFactory;
    private readonly IAddressFactory addressFactory;
    private readonly IFluentEmail fluentEmail;

    public EmailFactory(
        ISanitizationService sanitizationService,
        IAttachmentFactory attachmentFactory,
        IAddressFactory addressFactory,
        IFluentEmail fluentEmail)
    {
        this.sanitizationService = sanitizationService;
        this.attachmentFactory = attachmentFactory;
        this.addressFactory = addressFactory;
        this.fluentEmail = fluentEmail;
    }

    public IFluentEmail GetEmail(SendEmailArgs args)
    {
        var recipients = addressFactory.Create(args.Recipients);

        var sanitizedBody = sanitizationService.Sanitize(args.Body);
        var email = fluentEmail
            .To(recipients)
            .Subject(args.Subject)
            .Body(sanitizedBody, isHtml: true);

        if (args.Sender is not null)
            email.SetFrom(args.Sender.Email, args.Sender.Name);

        if (args.Attachments is not null)
        {
            var attachments = attachmentFactory.Create(args.Attachments);
            email.Attach(attachments);
        }

        return email;
    }
}