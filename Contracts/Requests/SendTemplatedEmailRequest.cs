using Microservice.Email.Contracts.Common;

namespace Microservice.Email.Contracts.Requests;

public sealed class SendTemplatedEmailRequest
{
    public required Sender Sender { get; init; }
    public required string[] Recipients { get; init; }
    public required string TemplateName { get; init; }
    public required dynamic Data { get; init; }
}