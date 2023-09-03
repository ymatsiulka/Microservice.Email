using ArchitectProg.Kernel.Extensions.Utils;
using Microservice.Email.Core.Contracts.Requests;
using Microservice.Email.Core.Contracts.Responses;

namespace Microservice.Email.Core.Services.Interfaces;

public interface IEmailService
{
    Task<Result<EmailResponse>> Send(AttachmentsWrapper<SendEmailRequest> request);
    Task<Result<EmailResponse>> SendTemplated(AttachmentsWrapper<SendTemplatedEmailRequest> request);
    Task<Result<EmailResponse>> SendWithFormFiles(FormFilesWrapper<SendEmailRequest> request);
    Task<Result<EmailResponse>> SendTemplatedWithFormFiles(FormFilesWrapper<SendTemplatedEmailRequest> request);
}
