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
using System.Reflection;
using IFluentEmailFactory = Microservice.Email.Core.Factories.Interfaces.IFluentEmailFactory;

namespace Microservice.Email.Core.Services;

public sealed class EmailService : IEmailService
{
    private readonly IFluentEmailFactory fluentEmailFactory;
    private readonly IAttachmentFactory attachmentFactory;
    private readonly IResultFactory resultFactory;
    private readonly IUnitOfWorkFactory unitOfWorkFactory;
    private readonly IRepository<EmailEntity> emailRepository;
    private readonly IEmailMapper emailMapper;
    private readonly IEmailFactory emailFactory;
    private readonly IRetryPolicyFactory retryPolicyFactory;

    public EmailService(
        IFluentEmailFactory fluentEmailFactory,
        IAttachmentFactory attachmentFactory,
        IResultFactory resultFactory,
        IUnitOfWorkFactory unitOfWorkFactory,
        IRepository<EmailEntity> emailRepository,
        IEmailMapper emailMapper,
        IEmailFactory emailFactory,
        IRetryPolicyFactory retryPolicyFactory)
    {
        this.fluentEmailFactory = fluentEmailFactory;
        this.attachmentFactory = attachmentFactory;
        this.resultFactory = resultFactory;
        this.unitOfWorkFactory = unitOfWorkFactory;
        this.emailRepository = emailRepository;
        this.emailMapper = emailMapper;
        this.emailFactory = emailFactory;
        this.retryPolicyFactory = retryPolicyFactory;
    }

    public async Task<Result<EmailResponse>> Send(SendEmailRequest request)
    {
        var email = fluentEmailFactory.GetEmail(request);
        var attachments = request.Attachments?.Select(attachmentFactory.Create).ToList() ?? new List<Attachment>();
        email.Attach(attachments);

        var policy = retryPolicyFactory.GetPolicy<SendResponse>();
        var emailResponse = await policy.ExecuteAsync(async () => await email.SendAsync());

        if (!emailResponse.Successful)
        {
            var errorMessage = string.Join(" ", emailResponse.ErrorMessages);
            return resultFactory.Failure<EmailResponse>(new EmailSendException(errorMessage));
        }

        var emailEntity = emailFactory.Create(request);
        using (var transaction = unitOfWorkFactory.BeginTransaction())
        {
            await emailRepository.Add(emailEntity);
            await transaction.Commit();
        }

        var response = emailMapper.Map(emailEntity);

        return response;
    }

        public async Task<Result<EmailResponse>> SendTemplated(SendTemplatedEmailRequest request)
        {
            var email = fluentEmailFactory.GetEmail(request);

            var policy = retryPolicyFactory.GetPolicy<SendResponse>();
            var emailResponse = await policy.ExecuteAsync(async () => await email.SendAsync());

            if (!emailResponse.Successful)
            {
                var errorMessage = string.Join(" ", emailResponse.ErrorMessages);
                return resultFactory.Failure<EmailResponse>(new EmailSendException(errorMessage));
            }
            //
            // //var emailEntity = emailFactory.Create(request);
            // var emailEntity = new EmailEntity()
            // {
            //     Recipients = request.Recipients,
            //     Sender = request.Sender.Email,
            //
            // }
            // using (var transaction = unitOfWorkFactory.BeginTransaction())
            // {
            //     await emailRepository.Add(emailEntity);
            //     await transaction.Commit();
            // }
            //
            // var response = emailMapper.Map(emailEntity);

            return null;
        }
    }
}