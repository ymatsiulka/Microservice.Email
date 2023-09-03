namespace Microservice.Email.Infrastructure.FileStorage.Contracts;

public sealed class RemoveResult
{
    public required string FileKey { get; init; }
}