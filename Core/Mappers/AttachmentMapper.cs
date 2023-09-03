using Microservice.Email.Core.Mappers.Interfaces;
using Microservice.Email.Infrastructure.FileStorage.Contracts;
using Microservice.Email.Infrastructure.Smtp.Contracts;

namespace Microservice.Email.Core.Mappers;

public sealed class AttachmentMapper : IAttachmentMapper
{
    public SendAttachmentArgs Map(IFormFile sourse)
    {
        var result = new SendAttachmentArgs
        {
            FileName = sourse.FileName,
            ContentType = sourse.ContentType,
            Length = sourse.Length,
            FileStream = sourse.OpenReadStream()
        };

        return result;
    }

    public SendAttachmentArgs Map(DownloadResult source)
    {
        var result = new SendAttachmentArgs
        {
            FileName = source.FileKey,
            ContentType = source.ContentType,
            Length = source.Size,
            FileStream = source.DownloadStream
        };

        return result;
    }
}