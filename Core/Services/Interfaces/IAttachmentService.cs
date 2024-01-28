using Microservice.Email.Infrastructure.FileStorage.Contracts;

namespace Microservice.Email.Core.Services.Interfaces;

public interface IAttachmentService
{
    Task<UploadResult[]> Upload(IFormFileCollection files);
    Task<InformationResult[]> Information(string[] files);
    Task<DownloadResult> Download(string fileName);
    Task<RemoveResult[]> Remove(string[] files);
}