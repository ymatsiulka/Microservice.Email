using Grpc.Contracts.Common;
using Microservice.Email.Core.Contracts.Requests;
using Yurutaru.Platform.NetCore.Core.Mappers.Interfaces;

namespace Microservice.Email.Grpc.Mappers.Interfaces;

public interface IAttachmentMapper : IMapper<GrpcAttachment, Attachment>
{
}