using ArchitectProg.Kernel.Extensions.Mappers;
using Grpc.Contracts.Email;
using Microservice.Email.Core.Contracts.Common;
using Microservice.Email.Core.Contracts.Requests;
using Microservice.Email.Grpc.Mappers.Interfaces;

namespace Microservice.Email.Grpc.Mappers;

public class SendTemplatedEmailRequestMapper :
    Mapper<GrpcSendTemplatedEmailRequest, SendTemplatedEmailRequest>,
    ISendTemplatedEmailRequestMapper
{
    public override SendTemplatedEmailRequest Map(GrpcSendTemplatedEmailRequest source)
    {
        var result = new SendTemplatedEmailRequest
        {
            TemplateName = source.TemplateName,
            TemplateProperties = source.TemplateProperties,
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