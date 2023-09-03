using FluentEmail.Core;
using Microservice.Email.Infrastructure.Smtp.Contracts;

namespace Microservice.Email.Infrastructure.Smtp.Factories.Interfaces;

public interface IEmailFactory
{
    IFluentEmail GetEmail(SendEmailArgs args);
}