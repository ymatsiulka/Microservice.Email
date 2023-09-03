using Minio;

namespace Microservice.Email.Infrastructure.FileStorage.Interfaces;

public interface IFileStorageConnectionProvider : IDisposable
{
    IMinioClient Client { get; }
    string Bucket { get; }
}