using Microservice.Email.Contracts.Common;

namespace Microservice.Email.Contracts.Requests;

public sealed class SendEmailRequest
{
    public required Sender Sender { get; init; }
    public required string[] Recipients { get; init; }
    public string? Body { get; init; }
    public required string Subject { get; init; }
}