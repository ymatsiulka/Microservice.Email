namespace Microservice.Email.Contracts.Common;

public sealed class Sender
{
    public required string Name { get; init; }
    public required string Email { get; init; }
}