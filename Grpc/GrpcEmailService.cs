using Microservice.Email.Core.Contracts.Requests;
using Microservice.Email.Core.Contracts.Responses;
using Microservice.Email.Core.Services.Interfaces;
using Microservice.Email.Grpc.Interfaces;

namespace Microservice.Email.Grpc;

public class GrpcEmailService : IGrpcEmailService
{
    private readonly IEmailService emailService;

    public GrpcEmailService(IEmailService emailService)
    {
        this.emailService = emailService;
    }

    public Task<EmailSendResponse> Send(SendEmailRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<EmailSendResponse> SendTemplated(SendTemplatedEmailRequest request)
    {
        throw new NotImplementedException();
    }
}