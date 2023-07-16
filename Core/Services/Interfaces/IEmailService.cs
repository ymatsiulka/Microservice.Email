using ArchitectProg.Kernel.Extensions.Utils;
using Microservice.Email.Contracts.Requests;
using Microservice.Email.Contracts.Responses;

namespace Microservice.Email.Core.Services.Interfaces;

public interface IEmailService
{
    Task<Result<EmailSendResponse>> Send(SendEmailRequest request);
}