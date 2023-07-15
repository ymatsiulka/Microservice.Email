using FluentEmail.Core.Models;
using Microservice.Email.Core.Factories.Interfaces;

namespace Microservice.Email.Core.Factories;

public sealed class AttachmentFactory : IAttachmentFactory
{
    public Attachment Create(IFormFile formFile)
    {
        var result = new Attachment
        {
            ContentType = formFile.ContentType,
            Data = formFile.OpenReadStream(),
            Filename = formFile.FileName,
            ContentId = Guid.NewGuid().ToString(),
            IsInline = true
        };

        return result;
    }
}
