using Microservice.Email.Contracts.Requests;
using Microservice.Email.Domain.Entities;

namespace Microservice.Email.Creators.Interfaces;

public interface IEmailCreator
{
    EmailEntity Create(SendEmailRequest request);
}