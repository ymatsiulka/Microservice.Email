using Microservice.Email.Core.Validators.Interfaces;

namespace Microservice.Email.Core.Validators;

public class FormFileAttachmentsValidator : IFormFileAttachmentsValidator
{
    public IEnumerable<string> Validate(IFormFileCollection attachments)
    {
        //file size should not exceed 30mb
        var haveInvalidAttachments = attachments.Any(attachment => attachment.Length > 30000000);
        if (haveInvalidAttachments)
            yield return "Attachments size must be lower than 30mb";
    }
}