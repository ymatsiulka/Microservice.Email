using Microservice.Email.Contracts.Requests.Base;
using Microservice.Email.Core.Extensions;
using Microservice.Email.Core.Validators.Interfaces;

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

        if (request.Attachments is not null)
        {
            //file size should not exceed 30mb
            var haveInvalidAttachments = request.Attachments.Any(attachment => attachment.Length > 30000000 );
            if (haveInvalidAttachments)
                yield return "Attachments size must be lower than 30mb";
        }
    }
}
