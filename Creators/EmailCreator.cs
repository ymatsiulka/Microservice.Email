using ArchitectProg.FunctionalExtensions.Services.Interfaces;
using Microservice.Email.Contracts.Requests;
using Microservice.Email.Creators.Interfaces;
using Microservice.Email.Domain.Entities;
using Microservice.Email.Domain.Enums;

namespace Microservice.Email.Creators
{
    public class EmailCreator : IEmailCreator
    {
        private readonly IDateTimeProvider dateTimeProvider;

        public EmailCreator(IDateTimeProvider dateTimeProvider)
        {
            this.dateTimeProvider = dateTimeProvider;
        }

        public EmailEntity Create(SendEmailRequest request)
        {
            var result = new EmailEntity
            {
                EmailStatus = EmailStatus.Sent,
                Recipients = request.Recipients,
                Subject = request.Subject,
                Sender = request.Sender,
                Body = request.Body,
                SentDate = dateTimeProvider.GetUtcNow(),
            };

            return result;
        }
    }
}
