using Microservice.Email.Core.Contracts.Common;

namespace Microservice.Email.Core.Contracts.Requests.Base;

public abstract class BaseEmailRequest
{
    public Sender? Sender { get; init; }
    public required string[] Recipients { get; init; }
    public IFormFileCollection? Attachments { get; init; }
}
