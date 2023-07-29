using ArchitectProg.Kernel.Extensions.Utils;
using Microservice.Email.Core.Contracts.Requests;
using Microservice.Email.Core.Contracts.Responses;

namespace Microservice.Email.Core.Services.Interfaces;

public interface IEmailService
{
    Task<Result<EmailResponse>> Send(SendEmailRequest request);
    Task<Result<EmailResponse>> SendTemplated(SendTemplatedEmailRequest request);
}
