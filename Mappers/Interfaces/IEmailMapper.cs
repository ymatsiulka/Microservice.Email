using ArchitectProg.Kernel.Extensions.Mappers.Interfaces;
using Microservice.Email.Contracts.Responses;
using Microservice.Email.Domain.Entities;

namespace Microservice.Email.Mappers.Interfaces;

public interface IEmailMapper : IMapper<EmailEntity, EmailResponse>
{
}