namespace Microservice.Email.Core.Contracts.Responses;

public sealed class RecipientResponse
{
    public int Id { get; set; }
    public required string Email { get; set; }
}
