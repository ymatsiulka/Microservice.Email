using Microservice.Email.Domain.Entities;
using Microservice.Email.Infrastructure.Smtp.Contracts;

namespace Microservice.Email.Core.Factories.Interfaces;

public interface IEmailEntityFactory
{
    EmailEntity Create(SendEmailArgs args);
}