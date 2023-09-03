using Microservice.Email.Core.Contracts.Common;
using Microservice.Email.Core.Contracts.Requests;
using Microservice.Email.Infrastructure.Smtp.Contracts;

namespace Microservice.Email.Core.Factories.Interfaces;

public interface ISendEmailArgsFactory
{
    SendEmailArgs Create(SendEmailRequest request, SendAttachmentArgs[] attachments);
    SendEmailArgs Create(SendTemplatedEmailRequest request, EmailContent content, SendAttachmentArgs[] attachments);
}