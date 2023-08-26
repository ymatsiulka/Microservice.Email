using ArchitectProg.Kernel.Extensions.Mappers;
using Google.Protobuf.WellKnownTypes;
using Grpc.Contracts.Common;
using Grpc.Contracts.Email;
using Microservice.Email.Core.Contracts.Responses;
using Microservice.Email.Grpc.Mappers.Interfaces;

namespace Microservice.Email.Grpc.Mappers;

public sealed class EmailResponseMapper :
    Mapper<EmailResponse, GrpcEmailResponse>,
    IEmailResponseMapper
{
    public override GrpcEmailResponse Map(EmailResponse source)
    {
        var result = new GrpcEmailResponse
        {
            Id = source.Id,
            Sender = new GrpcSender
            {
                Name = source.Sender.Name,
                Email = source.Sender.Email
            },
            EmailStatus = new GrpcEnumItem
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