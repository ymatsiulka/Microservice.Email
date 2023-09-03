using ArchitectProg.Kernel.Extensions.Utils;
using Microservice.Email.Core.Contracts.Responses;
using Microservice.Email.Infrastructure.Smtp.Contracts;

namespace Microservice.Email.Core.Services.Interfaces;

public interface ISendEmailService
{
    Task<Result<EmailResponse>> Send(SendEmailArgs args);
}