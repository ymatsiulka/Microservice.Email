using ArchitectProg.Kernel.Extensions.Mappers;
using Grpc.Contracts.Email;
using Microservice.Email.Core.Contracts.Common;
using Microservice.Email.Grpc.Mappers.Interfaces;

namespace Microservice.Email.Grpc.Mappers;

public class SendEmailRequestMapper :
    Mapper<SendEmailRequest, Core.Contracts.Requests.SendEmailRequest>,
    ISendEmailRequestMapper
{
    public override Core.Contracts.Requests.SendEmailRequest Map(SendEmailRequest source)
    {
        var result = new Core.Contracts.Requests.SendEmailRequest
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