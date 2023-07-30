using ArchitectProg.Kernel.Extensions.Exceptions;
using ArchitectProg.Kernel.Extensions.Utils;
using Microservice.Email.Core.Attributes;
using Microservice.Email.Core.Contracts.Requests;
using Microservice.Email.Core.Contracts.Responses;
using Microservice.Email.Core.Services.Interfaces;
using Microservice.Email.Core.Validators.Interfaces;
using Microservice.Email.Infrastructure.RabbitMQ.Interfaces;

namespace Microservice.Email.Core.Services;

public class EmailService : IEmailService
{
    private readonly ISendEmailService sendEmailService;
    private readonly ITemplatedEmailService templatedEmailService;
    private readonly ISendEmailRequestValidator sendEmailRequestValidator;
    private readonly ISendTemplatedEmailRequestValidator sendTemplatedEmailRequestValidator;
    
    public EmailService(
        ISendEmailService sendEmailService,
        ITemplatedEmailService templatedEmailService,
        ISendEmailRequestValidator sendEmailRequestValidator,
        ISendTemplatedEmailRequestValidator sendTemplatedEmailRequestValidator)
    {
        this.sendEmailService = sendEmailService;
        this.templatedEmailService = templatedEmailService;
        this.sendEmailRequestValidator = sendEmailRequestValidator;
        this.sendTemplatedEmailRequestValidator = sendTemplatedEmailRequestValidator;
    }

    [CounterMetric("Send email")]
    public async Task<Result<EmailResponse>> Send(SendEmailRequest request)
    {
        var errors = sendEmailRequestValidator.Validate(request).ToArray();
        if (errors.Any())
        {
            throw new ValidationException(string.Join(". ", errors));
        }

        var result = await sendEmailService.Send(request);
        return result;
    }
    
    [CounterMetric("Send templated email")]
    public async Task<Result<EmailResponse>> SendTemplated(SendTemplatedEmailRequest request)
    {
        var errors = sendTemplatedEmailRequestValidator.Validate(request).ToArray();
        if (errors.Any())
        {
            var message = string.Join(". ", errors);
            throw new ValidationException(message);
        }

        var sendRequest = await templatedEmailService.ProcessTemplatedRequest(request);
        var result = await sendEmailService.Send(sendRequest);
        return result;
    }
}
