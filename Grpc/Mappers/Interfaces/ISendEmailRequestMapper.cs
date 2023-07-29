using ArchitectProg.Kernel.Extensions.Mappers.Interfaces;
using Grpc.Contracts.Email;

namespace Microservice.Email.Grpc.Mappers.Interfaces;

public interface ISendEmailRequestMapper : IMapper<SendEmailRequest, Core.Contracts.Requests.SendEmailRequest>
{
}