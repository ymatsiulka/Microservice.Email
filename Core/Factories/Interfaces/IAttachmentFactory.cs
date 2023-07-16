using FluentEmail.Core.Models;

namespace Microservice.Email.Core.Factories.Interfaces;

public interface IAttachmentFactory
{
    Attachment Create(IFormFile attachment);
    Attachment[] Create(IFormFileCollection attachments);
}
