using FluentEmail.Core.Models;
using Microservice.Email.Core.Factories.Interfaces;

namespace Microservice.Email.Core.Factories;

public sealed class AttachmentFactory : IAttachmentFactory
{
    public Attachment Create(IFormFile attachment)
    {
        var result = new Attachment
        {
            ContentType = attachment.ContentType,
            Filename = attachment.FileName,
            Data = attachment.OpenReadStream(),
            ContentId = Guid.NewGuid().ToString(),
            IsInline = true
        };

        return result;
    }

    public Attachment[] Create(IFormFileCollection attachments)
    {
        var result = attachments
            .Select(Create)
            .ToArray();

        return result;
    }
}
