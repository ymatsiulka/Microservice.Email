using Grpc.Contracts.Email;
using Microservice.Email.Core.Contracts.Responses;
using Yurutaru.Platform.NetCore.Core.Mappers.Interfaces;

namespace Microservice.Email.Grpc.Mappers.Interfaces;

public interface IEmailResponseMapper : IMapper<EmailResponse, GrpcEmailResponse>
{
}