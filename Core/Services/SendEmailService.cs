using ArchitectProg.Kernel.Extensions.Interfaces;
using FluentEmail.Core.Models;
using Microservice.Email.Core.Contracts.Responses;
using Microservice.Email.Core.Exceptions;
using Microservice.Email.Core.Factories.Interfaces;
using Microservice.Email.Core.Mappers.Interfaces;
using Microservice.Email.Core.Services.Interfaces;
using Microservice.Email.Domain.Entities;
using Microservice.Email.Infrastructure.Smtp.Contracts;
using Microservice.Email.Infrastructure.Smtp.Interfaces;

namespace Microservice.Email.Core.Services;

public sealed class SendEmailService : ISendEmailService
{
    private readonly IEmailSender emailSender;
    private readonly IUnitOfWorkFactory unitOfWorkFactory;
    private readonly IRepository<EmailEntity> emailRepository;
    private readonly IEmailMapper emailMapper;
    private readonly IEmailEntityFactory emailEntityFactory;
    private readonly IRetryPolicyFactory retryPolicyFactory;

    public SendEmailService(
        IEmailSender emailSender,
        IUnitOfWorkFactory unitOfWorkFactory,
        IRepository<EmailEntity> emailRepository,
        IEmailEntityFactory emailEntityFactory,
        IRetryPolicyFactory retryPolicyFactory,
        IEmailMapper emailMapper)
    {
        this.emailSender = emailSender;
        this.unitOfWorkFactory = unitOfWorkFactory;
        this.emailRepository = emailRepository;
        this.emailEntityFactory = emailEntityFactory;
        this.retryPolicyFactory = retryPolicyFactory;
        this.emailMapper = emailMapper;
    }

    public async Task<EmailResponse> Send(SendEmailArgs args)
    {
        var policy = retryPolicyFactory.GetPolicy<SendResponse>();
        var sendResponse = await policy.ExecuteAsync(async () => await emailSender.Send(args));

        if (!sendResponse.Successful)
            throw new EmailSendException(sendResponse.ErrorMessages);

        var emailEntity = emailEntityFactory.Create(args);
        using (var transaction = unitOfWorkFactory.BeginTransaction())
        {
            await emailRepository.Add(emailEntity);
            await transaction.Commit();
        }

        var response = emailMapper.Map(emailEntity);

        return response;
    }
}