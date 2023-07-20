using Microservice.Email.Contracts.Requests.Base;

namespace Microservice.Email.Contracts.Requests;

public sealed class SendTemplatedEmailRequest : BaseEmailRequest
{
    public required string TemplateName { get; init; }
    public string? TemplateProperties { get; init; }
}