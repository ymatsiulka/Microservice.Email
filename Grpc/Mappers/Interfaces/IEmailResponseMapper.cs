using ArchitectProg.Kernel.Extensions.Mappers.Interfaces;
using Grpc.Contracts.Email;

namespace Microservice.Email.Grpc.Mappers.Interfaces;

public interface IEmailResponseMapper : IMapper<Core.Contracts.Responses.EmailResponse, EmailResponse>
{
}