using Google.Protobuf.WellKnownTypes;
using Grpc.Contracts.Common;
using Grpc.Contracts.Email;
using Microservice.Email.Core.Contracts.Responses;
using Microservice.Email.Grpc.Mappers.Interfaces;

namespace Microservice.Email.Grpc.Mappers;

public sealed class EmailResponseMapper : IEmailResponseMapper
{
    public GrpcEmailResponse Map(EmailResponse source)
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
            SentDate = source.SentDate.ToTimestamp()
        };
        
        var recipients = source.Recipients.Select(CreateRecipient).ToArray();
        result.Recipients.AddRange(recipients);
        
        return result;
    }

    private static GrpcRecipient CreateRecipient(RecipientResponse source) =>
    new()
    {
        Id = source.Id,
        Email = source.Email
    };
}