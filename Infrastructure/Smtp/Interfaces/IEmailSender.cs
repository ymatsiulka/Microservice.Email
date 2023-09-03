using FluentEmail.Core.Models;
using Microservice.Email.Infrastructure.Smtp.Contracts;

namespace Microservice.Email.Infrastructure.Smtp.Interfaces;

public interface IEmailSender
{
    Task<SendResponse> Send(SendEmailArgs args);
}