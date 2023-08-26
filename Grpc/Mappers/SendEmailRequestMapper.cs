using ArchitectProg.Kernel.Extensions.Mappers;
using Grpc.Contracts.Email;
using Microservice.Email.Core.Contracts.Common;
using Microservice.Email.Core.Contracts.Requests;
using Microservice.Email.Grpc.Mappers.Interfaces;

namespace Microservice.Email.Grpc.Mappers;

public sealed class SendEmailRequestMapper :
    Mapper<GrpcSendEmailRequest, SendEmailRequest>,
    ISendEmailRequestMapper
{
    public override SendEmailRequest Map(GrpcSendEmailRequest source)
    {
        var result = new SendEmailRequest
        {
            Body = source.Body,
            Subject = source.Subject,
            Recipients = source.Recipients.ToArray(),
            Sender = new Sender
            {
                Email = source.Sender.Email,
                Name = source.Sender.Name
            }
        };

        return result;
    }
}