using FluentEmail.Core.Models;
using Microservice.Email.Infrastructure.Smtp.Contracts;
using Microservice.Email.Infrastructure.Smtp.Factories.Interfaces;
using Microservice.Email.Infrastructure.Smtp.Interfaces;

namespace Microservice.Email.Infrastructure.Smtp;

public sealed class EmailSender : IEmailSender
{
    private readonly IEmailFactory emailFactory;

    public EmailSender(IEmailFactory emailFactory)
    {
        this.emailFactory = emailFactory;
    }

    public async Task<SendResponse> Send(SendEmailArgs args)
    {
        var email = emailFactory.GetEmail(args);
        var response = await email.SendAsync();
        return response;
    }
}