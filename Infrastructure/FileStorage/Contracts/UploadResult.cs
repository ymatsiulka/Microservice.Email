namespace Microservice.Email.Infrastructure.FileStorage.Contracts;

public sealed class UploadResult
{
    public required string FileKey { get; init; }
    public required string ContentType { get; init; }
    public required long Size { get; init; }
}