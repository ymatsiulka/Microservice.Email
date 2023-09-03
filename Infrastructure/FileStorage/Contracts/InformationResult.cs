namespace Microservice.Email.Infrastructure.FileStorage.Contracts;

public sealed class InformationResult
{
    public required string FileKey { get; init; }
    public required string ContentType { get; init; }
    public required long Size { get; init; }
    public required bool DeleteMarker { get; set; }
    public required DateTime LastModified { get; set; }
}