using FluentEmail.Core.Models;
using Microservice.Email.Infrastructure.Smtp.Contracts;
using Microservice.Email.Infrastructure.Smtp.Factories.Interfaces;

namespace Microservice.Email.Infrastructure.Smtp.Factories;

public sealed class AttachmentFactory : IAttachmentFactory
{
    public Attachment Create(SendAttachmentArgs args)
    {
        var result = new Attachment
        {
            ContentType = args.ContentType,
            Filename = args.FileName,
            Data = args.FileStream,
            IsInline = false
        };

        return result;
    }

    public Attachment[] Create(IEnumerable<SendAttachmentArgs> args)
    {
        var result = args
            .Select(Create)
            .ToArray();

        return result;
    }
}
