using Microservice.Email.Core.Contracts.Requests;
using Microservice.Email.Core.Contracts.Responses;

namespace Microservice.Email.Grpc.Interfaces;

public interface IGrpcEmailService
{
    Task<EmailResponse> Send(SendEmailRequest request);
    Task<EmailResponse> SendTemplated(SendTemplatedEmailRequest request);
}