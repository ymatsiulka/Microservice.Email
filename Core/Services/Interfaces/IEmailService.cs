using Microservice.Email.Core.Contracts.Requests;
using Microservice.Email.Core.Contracts.Responses;

namespace Microservice.Email.Core.Services.Interfaces;

public interface IEmailService
{
    Task<EmailResponse> Send(AttachmentsWrapper<SendEmailRequest> request);
    Task<EmailResponse> SendTemplated(AttachmentsWrapper<SendTemplatedEmailRequest> request);
    Task<EmailResponse> SendWithFormFiles(FormFilesWrapper<SendEmailRequest> request);
    Task<EmailResponse> SendTemplatedWithFormFiles(FormFilesWrapper<SendTemplatedEmailRequest> request);
}
