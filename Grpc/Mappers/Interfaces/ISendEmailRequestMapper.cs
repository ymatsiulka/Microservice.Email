using ArchitectProg.Kernel.Extensions.Mappers;
using Grpc.Contracts.Email;
using Microservice.Email.Core.Contracts.Requests;

namespace Microservice.Email.Grpc.Mappers.Interfaces;

public interface ISendEmailRequestMapper : IMapper<GrpcSendEmailRequest, AttachmentsWrapper<SendEmailRequest>>
{
}