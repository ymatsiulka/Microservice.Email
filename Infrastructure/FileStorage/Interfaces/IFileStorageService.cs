using Microservice.Email.Infrastructure.FileStorage.Contracts;

namespace Microservice.Email.Infrastructure.FileStorage.Interfaces;

public interface IFileStorageService
{
    Task<UploadResult> Upload(string fileName, string contentType, Stream file);
    Task<InformationResult> Information(string fileName);
    Task<DownloadResult> Download(string fileName);
    Task<RemoveResult> Remove(string fileName);
}