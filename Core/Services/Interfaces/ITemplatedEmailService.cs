using Microservice.Email.Contracts.Requests;

namespace Microservice.Email.Core.Services.Interfaces;

public interface ITemplatedEmailService
{
    Task<SendEmailRequest> ProcessTemplatedRequest(SendTemplatedEmailRequest request);
}