namespace Microservice.Email.Infrastructure.FileStorage.Contracts;

public sealed class DownloadResult
{
    public required string FileKey { get; set; }
    public required string ContentType { get; set; }
    public required long Size { get; set; }
    public required Stream DownloadStream { get; set; }
}