namespace Microservice.Email.Core.Validators.Interfaces;

public interface IEmailAddressValidator
{
    bool IsValid(string? email, string? displayName = null);
}
