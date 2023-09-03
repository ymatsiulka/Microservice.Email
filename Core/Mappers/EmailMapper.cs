using ArchitectProg.FunctionalExtensions.Factories.Interfaces;
using Microservice.Email.Core.Contracts.Common;
using Microservice.Email.Core.Contracts.Responses;
using Microservice.Email.Core.Mappers.Interfaces;
using Microservice.Email.Domain.Entities;

namespace Microservice.Email.Core.Mappers;

public sealed class EmailMapper : IEmailMapper
{
    private readonly IEnumItemFactory enumItemFactory;

    public EmailMapper(IEnumItemFactory enumItemFactory)
    {
        this.enumItemFactory = enumItemFactory;
    }

    public EmailResponse Map(EmailEntity source)
    {
        var sender = new Sender
        {
            Email = source.SenderEmail,
            Name = source.SenderName
        };

        var result = new EmailResponse
        {
            Id = source.Id,
            EmailStatus = enumItemFactory.GetEnumItem(source.EmailStatus),
            Recipients = source.Recipients,
            Sender = sender,
            SentDate = source.SentDate
        };

        return result;
    }
}