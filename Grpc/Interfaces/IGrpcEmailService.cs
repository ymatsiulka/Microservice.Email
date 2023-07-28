using Microservice.Email.Core.Contracts.Requests;
using Microservice.Email.Core.Contracts.Responses;

namespace Microservice.Email.Grpc.Interfaces;

public interface IGrpcEmailService
{
    Task<EmailSendResponse> Send(SendEmailRequest request);
    Task<EmailSendResponse> SendTemplated(SendTemplatedEmailRequest request);
}