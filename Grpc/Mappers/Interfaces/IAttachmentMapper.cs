using ArchitectProg.Kernel.Extensions.Mappers;
using Grpc.Contracts.Common;
using Microservice.Email.Core.Contracts.Requests;

namespace Microservice.Email.Grpc.Mappers.Interfaces;

public interface IAttachmentMapper : IMapper<GrpcAttachment, Attachment>
{
}