namespace Microservice.Email.Core.Contracts.Requests;

public sealed class Attachment
{
    public required string FileName { get; init; }
}