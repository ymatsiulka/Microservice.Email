using ArchitectProg.FunctionalExtensions.Services.Interfaces;
using Microservice.Email.Core.Factories.Interfaces;
using Microservice.Email.Domain.Entities;
using Microservice.Email.Domain.Enums;
using Microservice.Email.Infrastructure.Smtp.Contracts;
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

    public EmailEntity Create(SendEmailArgs args)
    {
        var defaultSender = smtpSettings.Host;
        var result = new EmailEntity
        {
            EmailStatus = EmailStatus.Sent,
            Subject = args.Subject,
            SenderName = args.Sender?.Name ?? defaultSender,
            SenderEmail = args.Sender?.Email ?? defaultSender,
            Body = args.Body,
            SentDate = dateTimeProvider.GetUtcNow()
        };

        return result;
    }
}