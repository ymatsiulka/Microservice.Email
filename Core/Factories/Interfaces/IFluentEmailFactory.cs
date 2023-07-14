using FluentEmail.Core;
using Microservice.Email.Contracts.Requests;

namespace Microservice.Email.Core.Factories.Interfaces;

public interface IFluentEmailFactory
{
    IFluentEmail GetEmail(SendEmailRequest request);
    IFluentEmail GetEmail(SendTemplatedEmailRequest request);
}