using Microservice.Email.Core.Validators.Interfaces;
using System.Net.Mail;

namespace Microservice.Email.Core.Validators;

public sealed class EmailAddressValidator : IEmailAddressValidator
{
    public bool IsValid(string? email, string? displayName = null)
    {
        var result = MailAddress.TryCreate(email, displayName, out _);
        return result;
    }
}
