using Microservice.Email.Core.Contracts.Requests;

namespace Microservice.Email.Core.Validators.Interfaces;

public interface ISendTemplatedEmailRequestValidator
{
    IEnumerable<string> Validate(AttachmentsWrapper<SendTemplatedEmailRequest> request);
    IEnumerable<string> Validate(FormFilesWrapper<SendTemplatedEmailRequest> request);
}
