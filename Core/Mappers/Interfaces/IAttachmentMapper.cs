using ArchitectProg.Kernel.Extensions.Mappers;
using Microservice.Email.Infrastructure.FileStorage.Contracts;
using Microservice.Email.Infrastructure.Smtp.Contracts;

namespace Microservice.Email.Core.Mappers.Interfaces;

public interface IAttachmentMapper : IMapper<IFormFile, SendAttachmentArgs>,
    IMapper<DownloadResult, SendAttachmentArgs>
{
}