using ArchitectProg.Kernel.Extensions.Mappers;
using Grpc.Contracts.Email;
using Microservice.Email.Core.Contracts.Common;
using Microservice.Email.Grpc.Mappers.Interfaces;

namespace Microservice.Email.Grpc.Mappers;

public class SendTemplatedEmailRequestMapper :
    Mapper<SendTemplatedEmailRequest, Core.Contracts.Requests.SendTemplatedEmailRequest>,
    ISendTemplatedEmailRequestMapper
{
    public override Core.Contracts.Requests.SendTemplatedEmailRequest Map(SendTemplatedEmailRequest source)
    {
        var result = new Core.Contracts.Requests.SendTemplatedEmailRequest
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