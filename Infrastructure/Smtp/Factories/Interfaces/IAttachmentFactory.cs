using Microservice.Email.Infrastructure.Smtp.Contracts;
using Attachment = FluentEmail.Core.Models.Attachment;

namespace Microservice.Email.Infrastructure.Smtp.Factories.Interfaces;

public interface IAttachmentFactory
{
    Attachment Create(SendAttachmentArgs args);
    Attachment[] Create(IEnumerable<SendAttachmentArgs> args);
}
