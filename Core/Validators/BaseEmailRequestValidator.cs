using Microservice.Email.Core.Contracts.Requests.Base;
using Microservice.Email.Core.Validators.Interfaces;
using Microservice.Email.Extensions;

namespace Microservice.Email.Core.Validators;

public sealed class BaseEmailRequestValidator : IBaseEmailRequestValidator
{
    private readonly ISenderValidator senderValidator;

    public BaseEmailRequestValidator(ISenderValidator senderValidator)
    {
        this.senderValidator = senderValidator;
    }

    public IEnumerable<string> Validate(BaseEmailRequest request)
    {
        if (request.Sender is not null)
        {
            var senderErrors = senderValidator.Validate(request.Sender);
            foreach (var senderError in senderErrors)
                yield return senderError;
        }

        if (request.Recipients.IsEmpty())
            yield return "Recipients are required to send email";

        var invalidRecipients = request.Recipients.Where(recipient => !recipient.IsValidEmail()).ToArray();
        if (invalidRecipients.Any())
        {
            var invalidEmails = string.Join(", ", invalidRecipients);
            yield return $"Invalid recipients found. Invalid emails: {invalidEmails}";
        }
    }
}
