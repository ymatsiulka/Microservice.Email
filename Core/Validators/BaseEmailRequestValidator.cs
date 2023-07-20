using Microservice.Email.Contracts.Requests.Base;
using Microservice.Email.Core.Validators.Interfaces;

namespace Microservice.Email.Core.Validators;

public sealed class BaseEmailRequestValidator : IBaseEmailRequestValidator
{
    private readonly IEmailAddressValidator emailAddressValidator;

    public BaseEmailRequestValidator(IEmailAddressValidator emailAddressValidator)
    {
        this.emailAddressValidator = emailAddressValidator;
    }

    public IList<string> Validate(BaseEmailRequest baseEmailRequest)
    {
        if (baseEmailRequest is null)
            throw new ArgumentNullException(nameof(baseEmailRequest));

        var errors = new List<string>();

        var sender = baseEmailRequest.Sender;

        if (sender is not null &&
            !emailAddressValidator.IsValid(sender.Email, sender.Name))
        {
            errors.Add($"Sender email={sender.Email} or name={sender.Name} are not valid!");
        }

        var invalidRecipients = baseEmailRequest.Recipients.Where(recipient => !emailAddressValidator.IsValid(recipient)).ToArray();
        if (invalidRecipients.Any())
        {
            errors.Add($"Recipient with emails are invalid: {string.Join(", ", invalidRecipients)}");
        }

        return errors;
    }
}
