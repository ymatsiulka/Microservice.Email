using ArchitectProg.Kernel.Extensions.Factories.Interfaces;
using ArchitectProg.Kernel.Extensions.Interfaces;
using ArchitectProg.Kernel.Extensions.Utils;
using FluentEmail.Core.Models;
using Microservice.Email.Contracts.Requests;
using Microservice.Email.Contracts.Responses;
using Microservice.Email.Core.Exceptions;
using Microservice.Email.Core.Factories.Interfaces;
using Microservice.Email.Core.Mappers.Interfaces;
using Microservice.Email.Core.Services.Interfaces;
using Microservice.Email.Domain.Entities;

namespace Microservice.Email.Core.Services;

public sealed class SendEmailService : ISendEmailService
{
    private readonly IEmailFactory emailFactory;
    private readonly IResultFactory resultFactory;
    private readonly IUnitOfWorkFactory unitOfWorkFactory;
    private readonly IRepository<EmailEntity> emailRepository;
    private readonly IEmailMapper emailMapper;
    private readonly IEmailEntityFactory emailEntityFactory;
    private readonly IRetryPolicyFactory retryPolicyFactory;

    public SendEmailService(
        IEmailFactory emailFactory,
        IResultFactory resultFactory,
        IUnitOfWorkFactory unitOfWorkFactory,
        IRepository<EmailEntity> emailRepository,
        IEmailEntityFactory emailEntityFactory,
        IRetryPolicyFactory retryPolicyFactory,
        IEmailMapper emailMapper)
    {
        this.emailFactory = emailFactory;
        this.resultFactory = resultFactory;
        this.unitOfWorkFactory = unitOfWorkFactory;
        this.emailRepository = emailRepository;
        this.emailEntityFactory = emailEntityFactory;
        this.retryPolicyFactory = retryPolicyFactory;
        this.emailMapper = emailMapper;
    }

    public async Task<Result<EmailSendResponse>> Send(SendEmailRequest request)
    {
        var email = emailFactory.GetEmail(request);

        var policy = retryPolicyFactory.GetPolicy<SendResponse>();
        var emailResponse = await policy.ExecuteAsync(async () => await email.SendAsync());

        if (!emailResponse.Successful)
            return resultFactory.Failure<EmailSendResponse>(new EmailSendException(emailResponse.ErrorMessages));

        var emailEntity = emailEntityFactory.Create(request);
        using (var transaction = unitOfWorkFactory.BeginTransaction())
        {
            await emailRepository.Add(emailEntity);
            await transaction.Commit();
        }

        var response = emailMapper.Map(emailEntity);

        return response;
    }
}