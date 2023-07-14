using System.Reflection;
using ArchitectProg.Kernel.Extensions.Factories.Interfaces;
using ArchitectProg.Kernel.Extensions.Interfaces;
using ArchitectProg.Kernel.Extensions.Utils;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using Microservice.Email.Contracts.Requests;
using Microservice.Email.Contracts.Responses;
using Microservice.Email.Core.Exceptions;
using Microservice.Email.Core.Factories.Interfaces;
using Microservice.Email.Core.Mappers.Interfaces;
using Microservice.Email.Core.Services.Interfaces;
using Microservice.Email.Domain.Entities;
using IFluentEmailFactory = Microservice.Email.Core.Factories.Interfaces.IFluentEmailFactory;

namespace Microservice.Email.Core.Services
{
    public sealed class EmailService : IEmailService
    {
        private readonly IFluentEmailFactory fluentEmailFactory;
        private readonly IResultFactory resultFactory;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IRepository<EmailEntity> emailRepository;
        private readonly IEmailMapper emailMapper;
        private readonly IEmailFactory emailFactory;
        private readonly IRetryPolicyFactory retryPolicyFactory;

        public EmailService(
            IFluentEmailFactory fluentEmailFactory,
            IResultFactory resultFactory,
            IUnitOfWorkFactory unitOfWorkFactory,
            IRepository<EmailEntity> emailRepository,
            IEmailMapper emailMapper,
            IEmailFactory emailFactory,
            IRetryPolicyFactory retryPolicyFactory)
        {
            this.fluentEmailFactory = fluentEmailFactory;
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
            foreach (var formFile in request.FormFiles ?? new FormFileCollection())
            {
                var attachment = new Attachment
                {
                    ContentType = formFile.ContentType,
                    Data = formFile.OpenReadStream(),
                    Filename = formFile.FileName,
                    ContentId = Guid.NewGuid().ToString(),
                    IsInline = true
                };

                email.Attach(attachment);
            }

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

        public Task<Result<EmailResponse>> SendTemplated(SendTemplatedEmailRequest request)
        {
            var assembly = Assembly.GetExecutingAssembly();
            //.UsingTemplateFromEmbedded("Microservice.Email.Templates.ExampleModel.cshtml", new { UserName = "John Doe" }, assembly);

            throw new NotImplementedException();
        }
    }
}