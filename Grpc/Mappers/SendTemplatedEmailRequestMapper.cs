using Grpc.Contracts.Email;
using Microservice.Email.Core.Contracts.Common;
using Microservice.Email.Core.Contracts.Requests;
using Microservice.Email.Grpc.Mappers.Interfaces;

namespace Microservice.Email.Grpc.Mappers;

public sealed class SendTemplatedEmailRequestMapper : ISendTemplatedEmailRequestMapper
{
    private readonly IAttachmentMapper attachmentMapper;

    public SendTemplatedEmailRequestMapper(IAttachmentMapper attachmentMapper)
    {
        this.attachmentMapper = attachmentMapper;
    }

    public AttachmentsWrapper<SendTemplatedEmailRequest> Map(GrpcSendTemplatedEmailRequest source)
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

        var result = new AttachmentsWrapper<SendTemplatedEmailRequest>
        {
            Email = new SendTemplatedEmailRequest
            {
                TemplateName = source.TemplateName,
                TemplateProperties = source.TemplateProperties,
                Recipients = source.Recipients.ToArray(),
                Sender = sender
            },
            Attachments = attachments
        };

        return result;
    }
}