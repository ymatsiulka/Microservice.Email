using ArchitectProg.Kernel.Extensions.Utils;
using Microservice.Email.Contracts.Requests;
using Microservice.Email.Contracts.Responses;

namespace Microservice.Email.Services.Interfaces;

public interface IEmailService
{
    Task<Result<EmailResponse>> Send(SendEmailRequest request);
}