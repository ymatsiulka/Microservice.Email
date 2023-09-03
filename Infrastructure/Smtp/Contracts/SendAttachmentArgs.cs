namespace Microservice.Email.Infrastructure.Smtp.Contracts;

public class SendAttachmentArgs
{
    public required string FileName { get; init; }
    public required long Length { get; init; }
    public required string ContentType { get; init; }
    public required Stream FileStream { get; init; }
}