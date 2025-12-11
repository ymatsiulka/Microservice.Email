using Grpc.Contracts.Email;
using Microservice.Email.Core.Contracts.Requests;
using Yurutaru.Platform.NetCore.Core.Mappers.Interfaces;

namespace Microservice.Email.Grpc.Mappers.Interfaces;

public interface ISendTemplatedEmailRequestMapper :
    IMapper<GrpcSendTemplatedEmailRequest, AttachmentsWrapper<SendTemplatedEmailRequest>>
{
}