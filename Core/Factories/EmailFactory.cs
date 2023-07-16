using FluentEmail.Core;
using Microservice.Email.Contracts.Requests;
using Microservice.Email.Core.Factories.Interfaces;

namespace Microservice.Email.Core.Factories;

public class EmailFactory : IEmailFactory
{
    private readonly IAttachmentFactory attachmentFactory;
    private readonly IAddressFactory addressFactory;
    private readonly IFluentEmail fluentEmail;

    public EmailFactory(
        IAttachmentFactory attachmentFactory,
        IAddressFactory addressFactory,
        IFluentEmail fluentEmail)
    {
        this.attachmentFactory = attachmentFactory;
        this.addressFactory = addressFactory;
        this.fluentEmail = fluentEmail;
    }

    public IFluentEmail GetEmail(SendEmailRequest request)
    {
        var recipients = addressFactory.Create(request.Recipients);

        var email = fluentEmail
            .To(recipients)
            .Subject(request.Subject)
            .Body(request.Body, isHtml: true);

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