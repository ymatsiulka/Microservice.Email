using ArchitectProg.Kernel.Extensions.Interfaces;
using ArchitectProg.Kernel.Extensions.Utils;
using FluentEmail.Core;
using Microservice.Email.Contracts.Requests;
using Microservice.Email.Contracts.Responses;
using Microservice.Email.Creators.Interfaces;
using Microservice.Email.Domain.Entities;
using Microservice.Email.Factories.Interfaces;
using Microservice.Email.Mappers.Interfaces;
using Microservice.Email.Services.Interfaces;

namespace Microservice.Email.Services;

public sealed class EmailService : IEmailService
{
    private readonly IFluentEmailFactory fluentEmailFactory;
    private readonly IUnitOfWorkFactory unitOfWorkFactory;
    private readonly IAddressFactory addressFactory;
    private readonly IRepository<EmailEntity> emailRepository;
    private readonly IEmailMapper emailMapper;
    private readonly IEmailCreator emailCreator;

    public EmailService(IFluentEmailFactory fluentEmailFactory,
        IUnitOfWorkFactory unitOfWorkFactory,
        IAddressFactory addressFactory,
        IRepository<EmailEntity> emailRepository,
        IEmailMapper emailMapper,
        IEmailCreator emailCreator)
    {
        this.fluentEmailFactory = fluentEmailFactory;
        this.unitOfWorkFactory = unitOfWorkFactory;
        this.addressFactory = addressFactory;
        this.emailRepository = emailRepository;
        this.emailMapper = emailMapper;
        this.emailCreator = emailCreator;
    }

    public async Task<Result<EmailResponse>> Send(SendEmailRequest request)
    {
        var recipients = request.Recipients
            .Select(addressFactory.CreateAddress)
            .ToArray();

        var email = fluentEmailFactory.Create()
            .To(recipients)
            .Subject(request.Subject)
            .Body(request.Body);

        var emailResponse = await email.SendAsync();

        //var stringEmailErrorMessages = string.Join(" ", emailResponse.ErrorMessages);

        var emailEntity = emailCreator.Create(request);

        using (var transaction = unitOfWorkFactory.BeginTransaction())
        {
            await emailRepository.Add(emailEntity);
            await transaction.Commit();
        }

        var response = emailMapper.Map(emailEntity);

        return response;
    }
}