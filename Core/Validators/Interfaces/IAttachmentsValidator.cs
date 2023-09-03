using Microservice.Email.Core.Contracts.Requests;

namespace Microservice.Email.Core.Validators.Interfaces;

public interface IAttachmentsValidator
{
    IEnumerable<string> Validate(IEnumerable<Attachment> attachments);
}