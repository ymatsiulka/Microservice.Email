namespace Microservice.Email.Core.Contracts.Common;

public class EmailContent
{
    public required string Body { get; init; }
    public required string Subject { get; init; }
}