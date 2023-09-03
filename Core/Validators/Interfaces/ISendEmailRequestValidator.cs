using Microservice.Email.Core.Contracts.Requests;

namespace Microservice.Email.Core.Validators.Interfaces;

public interface ISendEmailRequestValidator
{
    IEnumerable<string> Validate(AttachmentsWrapper<SendEmailRequest> request);
    IEnumerable<string> Validate(FormFilesWrapper<SendEmailRequest> request);
}
