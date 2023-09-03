namespace Microservice.Email.Core.Validators.Interfaces;

public interface IFormFileAttachmentsValidator
{
    IEnumerable<string> Validate(IFormFileCollection attachments);
}