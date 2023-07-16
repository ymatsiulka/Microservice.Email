using FluentEmail.Core;
using Microservice.Email.Contracts.Requests;

namespace Microservice.Email.Core.Factories.Interfaces;

public interface IEmailFactory
{
    IFluentEmail GetEmail(SendEmailRequest request);
}