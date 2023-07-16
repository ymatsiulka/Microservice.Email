using Microservice.Email.Contracts.Common;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Email.Contracts.Requests;

public sealed class SendTemplatedEmailRequest
{
    public required Sender? Sender { get; init; }
    public required string[] Recipients { get; init; }
    public required string TemplateName { get; init; }
    public string? TemplateProperties { get; init; }
    public IFormFileCollection? Attachments { get; init; }
}