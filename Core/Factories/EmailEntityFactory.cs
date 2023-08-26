using ArchitectProg.FunctionalExtensions.Services.Interfaces;
using Microservice.Email.Core.Contracts.Requests;
using Microservice.Email.Core.Factories.Interfaces;
using Microservice.Email.Domain.Entities;
using Microservice.Email.Domain.Enums;
using Microservice.Email.Infrastructure.Smtp.Settings;
using Microsoft.Extensions.Options;

namespace Microservice.Email.Core.Factories;

public sealed class EmailEntityFactory : IEmailEntityFactory
{
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly SmtpSettings smtpSettings;

    public EmailEntityFactory(
        IDateTimeProvider dateTimeProvider,
        IOptions<SmtpSettings> smtpSettings)
    {
        this.dateTimeProvider = dateTimeProvider;
        this.smtpSettings = smtpSettings.Value;
    }

    public EmailEntity Create(SendEmailRequest request)
    {
        var defaultSender = smtpSettings.Host;
        var result = new EmailEntity
        {
            EmailStatus = EmailStatus.Sent,
            Recipients = request.Recipients,
            Subject = request.Subject,
            SenderName = request.Sender?.Name ?? defaultSender,
            SenderEmail = request.Sender?.Email ?? defaultSender,
            Body = request.Body,
            SentDate = dateTimeProvider.GetUtcNow()
        };

        return result;
    }
}