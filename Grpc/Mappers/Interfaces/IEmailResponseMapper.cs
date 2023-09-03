using ArchitectProg.Kernel.Extensions.Mappers;
using Grpc.Contracts.Email;
using Microservice.Email.Core.Contracts.Responses;

namespace Microservice.Email.Grpc.Mappers.Interfaces;

public interface IEmailResponseMapper : IMapper<EmailResponse, GrpcEmailResponse>
{
}