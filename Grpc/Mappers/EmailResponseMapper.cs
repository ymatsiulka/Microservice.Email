using ArchitectProg.Kernel.Extensions.Mappers;
using Google.Protobuf.WellKnownTypes;
using Grpc.Contracts.Common;
using Grpc.Contracts.Email;
using Microservice.Email.Grpc.Mappers.Interfaces;

namespace Microservice.Email.Grpc.Mappers;

public class EmailResponseMapper :
    Mapper<Core.Contracts.Responses.EmailResponse, EmailResponse>,
    IEmailResponseMapper
{
    public override EmailResponse Map(Core.Contracts.Responses.EmailResponse source)
    {
        var result = new EmailResponse
        {
            Id = source.Id,
            Sender = new Sender
            {
                Name = source.Sender.Name,
                Email = source.Sender.Email
            },
            EmailStatus = new EnumItem
            {
                Id = source.EmailStatus.Id,
                Name = source.EmailStatus.Name
            },
            Recipients = { source.Recipients },
            SentDate = source.SentDate.ToTimestamp()
        };

        return result;
    }
}