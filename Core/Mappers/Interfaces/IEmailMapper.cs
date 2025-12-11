using Microservice.Email.Core.Contracts.Responses;
using Microservice.Email.Domain.Entities;
using Yurutaru.Platform.NetCore.Core.Mappers.Interfaces;

namespace Microservice.Email.Core.Mappers.Interfaces;

public interface IEmailMapper : IMapper<EmailEntity, EmailResponse>
{
}