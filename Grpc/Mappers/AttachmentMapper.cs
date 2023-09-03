
using Grpc.Contracts.Common;
using Microservice.Email.Core.Contracts.Requests;
using Microservice.Email.Grpc.Mappers.Interfaces;

namespace Microservice.Email.Grpc.Mappers;

public class AttachmentMapper : IAttachmentMapper
{
    public Attachment Map(GrpcAttachment source)
    {
        var result = new Attachment
        {
            FileName = source.FileName
        };

        return result;
    }
}