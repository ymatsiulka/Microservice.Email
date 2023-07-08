using ArchitectProg.Kernel.Extensions.Utils;
using Microservice.Email.Contracts.Requests;
using Microservice.Email.Contracts.Responses;
using Microservice.Email.Services.Interfaces;

namespace Microservice.Email.Services
{
    public sealed class EmailService : IEmailService
    {
        public Task<Result<EmailResponse>> Send(SendEmailRequest request)
        {

            return null;
        }
    }
}
