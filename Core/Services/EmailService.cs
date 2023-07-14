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

namespace Microservice.Email.Core.Services
{
    public sealed class EmailService : IEmailService
    {
        private readonly IFluentEmail fluentEmail;
        private readonly IResultFactory resultFactory;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IAddressFactory addressFactory;
        private readonly IRepository<EmailEntity> emailRepository;
        private readonly IEmailMapper emailMapper;
        private readonly IEmailFactory emailFactory;
        private readonly IRetryPolicyFactory retryPolicyFactory;

        public EmailService(
            IFluentEmail fluentEmail,
            IResultFactory resultFactory,
            IUnitOfWorkFactory unitOfWorkFactory,
            IAddressFactory addressFactory,
            IRepository<EmailEntity> emailRepository,
            IEmailMapper emailMapper,
            IEmailFactory emailFactory,
            IRetryPolicyFactory retryPolicyFactory)
        {
            this.fluentEmail = fluentEmail;
            this.resultFactory = resultFactory;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.addressFactory = addressFactory;
            this.emailRepository = emailRepository;
            this.emailMapper = emailMapper;
            this.emailFactory = emailFactory;
            this.retryPolicyFactory = retryPolicyFactory;
        }

        public async Task<Result<EmailResponse>> Send(SendEmailRequest request)
        {
            var recipients = request.Recipients
                .Select(addressFactory.CreateAddress)
                .ToArray();

            var email = fluentEmail
                .SetFrom(request.Sender.Email, request.Sender.Name)
                .To(recipients)
                .Subject(request.Subject)
                .Body(request.Body);

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