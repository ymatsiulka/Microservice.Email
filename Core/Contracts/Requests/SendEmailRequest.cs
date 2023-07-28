using Microservice.Email.Core.Contracts.Requests.Base;

namespace Microservice.Email.Core.Contracts.Requests;

public sealed class SendEmailRequest : BaseEmailRequest
{
    public required string Body { get; init; }
    public required string Subject { get; init; }
}