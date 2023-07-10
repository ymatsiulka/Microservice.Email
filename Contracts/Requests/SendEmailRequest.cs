namespace Microservice.Email.Contracts.Requests;

public sealed class SendEmailRequest
{
    public string? Body { get; init; }
    public required string Subject { get; init; }
    public required string[] Recipients { get; init; }
}