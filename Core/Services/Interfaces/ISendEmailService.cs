using Microservice.Email.Core.Contracts.Responses;
using Microservice.Email.Infrastructure.Smtp.Contracts;

namespace Microservice.Email.Core.Services.Interfaces;

public interface ISendEmailService
{
    Task<EmailResponse> Send(SendEmailArgs args);
}