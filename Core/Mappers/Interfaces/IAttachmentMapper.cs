using Microservice.Email.Infrastructure.FileStorage.Contracts;
using Microservice.Email.Infrastructure.Smtp.Contracts;
using Yurutaru.Platform.NetCore.Core.Mappers.Interfaces;

namespace Microservice.Email.Core.Mappers.Interfaces;

public interface IAttachmentMapper : IMapper<IFormFile, SendAttachmentArgs>,
    IMapper<DownloadResult, SendAttachmentArgs>
{
}