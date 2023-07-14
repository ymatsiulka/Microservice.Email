using System.Reflection;
using ArchitectProg.Kernel.Extensions.Interfaces;
using ArchitectProg.Kernel.Extensions.Utils;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using Microservice.Email.Contracts.Requests;
using Microservice.Email.Contracts.Responses;
using Microservice.Email.Core.Factories.Interfaces;
using Microservice.Email.Core.Mappers.Interfaces;
using Microservice.Email.Core.Services.Interfaces;
using Microservice.Email.Domain.Entities;

namespace Microservice.Email.Core.Services
{
    public sealed class EmailService : IEmailService
    {
        private readonly IFluentEmail fluentEmail;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IAddressFactory addressFactory;
        private readonly IRepository<EmailEntity> emailRepository;
        private readonly IEmailMapper emailMapper;
        private readonly IEmailFactory emailFactory;
        private readonly IRetryPolicyFactory retryPolicyFactory;

        public EmailService(
            IFluentEmail fluentEmail,
            IUnitOfWorkFactory unitOfWorkFactory,
            IAddressFactory addressFactory,
            IRepository<EmailEntity> emailRepository,
            IEmailMapper emailMapper,
            IEmailFactory emailFactory,
            IRetryPolicyFactory retryPolicyFactory)
        {
            this.fluentEmail = fluentEmail;
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

            var assembly = Assembly.GetExecutingAssembly();

            var email = fluentEmail
                .Subject(request.Subject)
                .Body(request.Body)
                .To(recipients);
            //.UsingTemplateFromEmbedded("Microservice.Email.Templates.ExampleModel.cshtml", new { UserName = "John Doe" }, assembly);

            var policy = retryPolicyFactory.GetPolicy<SendResponse>();
            var emailResponse = await policy.ExecuteAsync(async () => await email.SendAsync());

            //var stringEmailErrorMessages = string.Join(" ", emailResponse.ErrorMessages);

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
            throw new NotImplementedException();
        }
    }
}