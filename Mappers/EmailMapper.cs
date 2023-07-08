using ArchitectProg.FunctionalExtensions.Factories.Interfaces;
using ArchitectProg.Kernel.Extensions.Mappers;
using Microservice.Email.Contracts.Responses;
using Microservice.Email.Domain.Entities;
using Microservice.Email.Mappers.Interfaces;

namespace Microservice.Email.Mappers
{
    public sealed class EmailMapper : Mapper<EmailEntity, EmailResponse>, IEmailMapper
    {
        private readonly IEnumItemFactory enumItemFactory;

        public EmailMapper(IEnumItemFactory enumItemFactory)
        {
            this.enumItemFactory = enumItemFactory;
        }

        public override EmailResponse Map(EmailEntity source)
        {
            var result = new EmailResponse
            {
                Id = source.Id,
                EmailStatus = enumItemFactory.GetEnumItem(source.EmailStatus),
                Body = source.Body,
                Recipients = source.Recipients,
                Sender = source.Sender,
                SentDate = source.SentDate,
                Subject = source.Subject
            };

            return result;
        }
    }
}
