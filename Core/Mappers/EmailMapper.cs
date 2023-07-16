using ArchitectProg.FunctionalExtensions.Factories.Interfaces;
using ArchitectProg.Kernel.Extensions.Mappers;
using Microservice.Email.Contracts.Common;
using Microservice.Email.Contracts.Responses;
using Microservice.Email.Core.Mappers.Interfaces;
using Microservice.Email.Domain.Entities;

namespace Microservice.Email.Core.Mappers;

public sealed class EmailMapper : Mapper<EmailEntity, EmailSendResponse>, IEmailMapper
{
    private readonly IEnumItemFactory enumItemFactory;

    public EmailMapper(IEnumItemFactory enumItemFactory)
    {
        this.enumItemFactory = enumItemFactory;
    }

    public override EmailSendResponse Map(EmailEntity source)
    {
        var sender = new Sender
        {
            Email = source.SenderEmail,
            Name = source.SenderName
        };

        var result = new EmailSendResponse
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