using Microservice.Email.Core.Contracts.Common;

namespace Microservice.Email.Infrastructure.Smtp.Contracts;

public class SendEmailArgs
{
    public Sender? Sender { get; init; }
    public required string[] Recipients { get; init; }
    public required string Body { get; init; }
    public required string Subject { get; init; }
    public IEnumerable<SendAttachmentArgs>? Attachments { get; init; }
}