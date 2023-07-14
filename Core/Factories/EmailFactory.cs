using ArchitectProg.FunctionalExtensions.Services.Interfaces;
using Microservice.Email.Contracts.Requests;
using Microservice.Email.Core.Factories.Interfaces;
using Microservice.Email.Domain.Entities;
using Microservice.Email.Domain.Enums;
using Microservice.Email.Smtp;
using Microsoft.Extensions.Options;

namespace Microservice.Email.Core.Factories
{
    public class EmailFactory : IEmailFactory
    {
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly SmtpSettings smtpSettings;

        public EmailFactory(IDateTimeProvider dateTimeProvider,
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
}