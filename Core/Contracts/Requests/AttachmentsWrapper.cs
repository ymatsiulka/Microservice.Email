using Microservice.Email.Core.Contracts.Requests.Base;

namespace Microservice.Email.Core.Contracts.Requests;

public class AttachmentsWrapper<T> where T : BaseEmailRequest
{
    public required T Email { get; init; }
    public IEnumerable<Attachment>? Attachments { get; init; }
}