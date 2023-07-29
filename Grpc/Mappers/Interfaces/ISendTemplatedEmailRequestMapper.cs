using ArchitectProg.Kernel.Extensions.Mappers.Interfaces;
using Grpc.Contracts.Email;

namespace Microservice.Email.Grpc.Mappers.Interfaces;

public interface ISendTemplatedEmailRequestMapper :
    IMapper<SendTemplatedEmailRequest, Core.Contracts.Requests.SendTemplatedEmailRequest>
{
}