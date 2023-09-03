using Microservice.Email.Core.Contracts.Common;
using Microservice.Email.Core.Contracts.Requests;

namespace Microservice.Email.Core.Services.Interfaces;

public interface ITemplateProcessingService
{
    Task<EmailContent> Process(SendTemplatedEmailRequest request);
}