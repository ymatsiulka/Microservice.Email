using Microservice.Email.Contracts.Requests;
using Microservice.Email.Domain.Entities;

namespace Microservice.Email.Core.Factories.Interfaces;

public interface IEmailEntityFactory
{
    EmailEntity Create(SendEmailRequest request);
}