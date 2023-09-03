using Microservice.Email.Core.Contracts.Common;
using Microservice.Email.Core.Contracts.Requests;
using Microservice.Email.Core.Factories.Interfaces;
using Microservice.Email.Infrastructure.Smtp.Contracts;

namespace Microservice.Email.Core.Factories;

public class SendEmailArgsFactory : ISendEmailArgsFactory
{
    public SendEmailArgs Create(SendEmailRequest request, SendAttachmentArgs[] attachments)
    {
        var result = new SendEmailArgs
        {
            Sender = request.Sender,
            Recipients = request.Recipients,
            Subject = request.Subject,
            Body = request.Body,
            Attachments = attachments
        };

        return result;
    }

    public SendEmailArgs Create(
        SendTemplatedEmailRequest request,
        EmailContent content,
        SendAttachmentArgs[] attachments)
    {
        var result = new SendEmailArgs
        {
            Sender = request.Sender,
            Recipients = request.Recipients,
            Subject = content.Subject,
            Body = content.Body,
            Attachments = attachments
        };

        return result;
    }
}