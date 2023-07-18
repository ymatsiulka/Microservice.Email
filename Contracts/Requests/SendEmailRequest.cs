using Microservice.Email.Contracts.Common;
using Microservice.Email.Contracts.Requests.Base;

namespace Microservice.Email.Contracts.Requests;

public sealed class SendEmailRequest : BaseEmailRequest
{
    public required string Body { get; init; }
    public required string Subject { get; init; }
}