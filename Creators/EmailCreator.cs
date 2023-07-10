using ArchitectProg.FunctionalExtensions.Services.Interfaces;
using Microservice.Email.Contracts.Requests;
using Microservice.Email.Creators.Interfaces;
using Microservice.Email.Domain.Entities;
using Microservice.Email.Domain.Enums;
using Microservice.Email.Settings;
using Microsoft.Extensions.Options;

namespace Microservice.Email.Creators;

public class EmailCreator : IEmailCreator
{
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly SmtpSettings smtpSettings;

    public EmailCreator(IDateTimeProvider dateTimeProvider,
        IOptions<SmtpSettings> smtpSettings)
    {
        this.dateTimeProvider = dateTimeProvider;
        this.smtpSettings = smtpSettings.Value;
    }

    public EmailEntity Create(SendEmailRequest request)
    {
        var result = new EmailEntity
        {
            EmailStatus = EmailStatus.Sent,
            Recipients = request.Recipients,
            Subject = request.Subject,
            Sender = smtpSettings.Username ?? string.Empty,
            Body = request.Body,
            SentDate = dateTimeProvider.GetUtcNow(),
        };

        return result;
    }
}