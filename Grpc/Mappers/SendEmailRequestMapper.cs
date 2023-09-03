using Grpc.Contracts.Email;
using Microservice.Email.Core.Contracts.Common;
using Microservice.Email.Core.Contracts.Requests;
using Microservice.Email.Grpc.Mappers.Interfaces;

namespace Microservice.Email.Grpc.Mappers;

public sealed class SendEmailRequestMapper : ISendEmailRequestMapper
{
    private readonly IAttachmentMapper attachmentMapper;

    public SendEmailRequestMapper(IAttachmentMapper attachmentMapper)
    {
        this.attachmentMapper = attachmentMapper;
    }

    public AttachmentsWrapper<SendEmailRequest> Map(GrpcSendEmailRequest source)
    {
        var attachments = source.Attachments is null
            ? Array.Empty<Attachment>()
            : attachmentMapper.MapCollection(source.Attachments);

        var sender = source.Sender is null
            ? null
            : new Sender
            {
                Email = source.Sender.Email,
                Name = source.Sender.Name
            };

        var result = new AttachmentsWrapper<SendEmailRequest>
        {
            Email = new SendEmailRequest
            {
                Body = source.Body,
                Subject = source.Subject,
                Recipients = source.Recipients.ToArray(),
                Sender = sender
            },
            Attachments = attachments
        };

        return result;
    }
}