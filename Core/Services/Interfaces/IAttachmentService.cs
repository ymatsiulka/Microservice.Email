using ArchitectProg.Kernel.Extensions.Utils;
using Microservice.Email.Infrastructure.FileStorage.Contracts;

namespace Microservice.Email.Core.Services.Interfaces;

public interface IAttachmentService
{
    Task<Result<UploadResult[]>> Upload(IFormFileCollection files);
    Task<Result<InformationResult[]>> Information(string[] files);
    Task<Result<DownloadResult>> Download(string fileName);
    Task<Result<RemoveResult[]>> Remove(string[] files);
}