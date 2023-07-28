using FluentEmail.Core;
using Microservice.Email.Core.Contracts.Requests;
using Microservice.Email.Core.Factories.Interfaces;
using Microservice.Email.Core.Services.Interfaces;

namespace Microservice.Email.Core.Factories;

public class EmailFactory : IEmailFactory
{
    private readonly IHtmlSanitizationService htmlSanitizationService;
    private readonly IAttachmentFactory attachmentFactory;
    private readonly IAddressFactory addressFactory;
    private readonly IFluentEmail fluentEmail;

    public EmailFactory(
        IHtmlSanitizationService htmlSanitizationService,
        IAttachmentFactory attachmentFactory,
        IAddressFactory addressFactory,
        IFluentEmail fluentEmail)
    {
        this.htmlSanitizationService = htmlSanitizationService;
        this.attachmentFactory = attachmentFactory;
        this.addressFactory = addressFactory;
        this.fluentEmail = fluentEmail;
    }

    public IFluentEmail GetEmail(SendEmailRequest request)
    {
        var recipients = addressFactory.Create(request.Recipients);

        var sanitizedBody = htmlSanitizationService.Sanitize(request.Body);
        var email = fluentEmail
            .To(recipients)
            .Subject(request.Subject)
            .Body(sanitizedBody, isHtml: true);

        if (request.Sender is not null)
            email.SetFrom(request.Sender.Email, request.Sender.Name);

        if (request.Attachments is not null)
        {
            var attachments = attachmentFactory.Create(request.Attachments);
            email.Attach(attachments);
        }

        return email;
    }
}