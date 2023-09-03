using Microservice.Email.Core.Contracts.Requests;
using Microservice.Email.Core.Validators.Interfaces;

namespace Microservice.Email.Core.Validators;

public class AttachmentsValidator : IAttachmentsValidator
{
    public IEnumerable<string> Validate(IEnumerable<Attachment> attachments)
    {
        if (attachments.Any(x => string.IsNullOrWhiteSpace(x.FileName) || x.FileName is { Length: > 1024 }))
            yield return "Invalid attachments. Attachments should be not empty and have length less than 1024 characters";
    }
}