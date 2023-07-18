﻿using ArchitectProg.Kernel.Extensions.Exceptions;
using ArchitectProg.Kernel.Extensions.Utils;
using Microservice.Email.Contracts.Requests;
using Microservice.Email.Contracts.Responses;
using Microservice.Email.Core.Services.Interfaces;
using Microservice.Email.Core.Validators.Interfaces;

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

    public async Task<Result<EmailSendResponse>> Send(SendEmailRequest request)
    {
        var errors = sendEmailRequestValidator.Validate(request);
        if (errors.Any())
        {
            var message = string.Join("\n", errors);
            throw new ValidationException(message);
        }

        var result = await sendEmailService.Send(request);
        return result;
    }

    public async Task<Result<EmailSendResponse>> SendTemplate(SendTemplatedEmailRequest request)
    {
        var errors = sendTemplatedEmailRequestValidator.Validate(request);
        if (errors.Any())
        {
            var message = string.Join("\n", errors);
            throw new ValidationException(message);
        }

        var sendRequest = await templatedEmailService.ProcessTemplatedRequest(request);
        var result = await sendEmailService.Send(sendRequest);
        return result;
    }
}
