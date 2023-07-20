namespace Microservice.Email.Contracts.Common;

public sealed class Sender
{
    public string? Name { get; init; }
    public required string Email { get; init; }
}