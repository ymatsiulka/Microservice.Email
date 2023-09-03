namespace Microservice.Email.Infrastructure.FileStorage.Settings;

public sealed class FileStorageSettings
{
    public required string Endpoint { get; init; }
    public required string AccessKey { get; init; }
    public required string SecretKey { get; init; }
    public required string Bucket { get; init; }
    public required bool EnableSSL { get; init; }
}