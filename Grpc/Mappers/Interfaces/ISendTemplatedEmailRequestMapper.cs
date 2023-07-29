using ArchitectProg.Kernel.Extensions.Mappers.Interfaces;
using Grpc.Contracts.Email;
using Microservice.Email.Core.Contracts.Requests;

namespace Microservice.Email.Grpc.Mappers.Interfaces;

public interface ISendTemplatedEmailRequestMapper :
    IMapper<GrpcSendTemplatedEmailRequest, SendTemplatedEmailRequest>
{
}