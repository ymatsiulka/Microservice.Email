using Microservice.Email.Core.Contracts.Requests.Base;

namespace Microservice.Email.Core.Contracts.Requests;

public class FormFilesWrapper<T> where T : BaseEmailRequest
{
    public required T Email { get; init; }
    public IFormFileCollection? Attachments { get; init; }
}