using ArchitectProg.FunctionalExtensions.Extensions;
using Microservice.Email.Core.Contracts.Requests;
using Microservice.Email.Core.Validators.Interfaces;

namespace Microservice.Email.Core.Validators;

public sealed class SendEmailRequestValidator : ISendEmailRequestValidator
{
    private readonly IBaseEmailRequestValidator baseEmailRequestValidator;
    private readonly IFormFileAttachmentsValidator formFileAttachmentsValidator;
    private readonly IAttachmentsValidator attachmentsValidator;

    public SendEmailRequestValidator(
        IBaseEmailRequestValidator baseEmailRequestValidator,
        IFormFileAttachmentsValidator formFileAttachmentsValidator,
        IAttachmentsValidator attachmentsValidator)
    {
        this.baseEmailRequestValidator = baseEmailRequestValidator;
        this.formFileAttachmentsValidator = formFileAttachmentsValidator;
        this.attachmentsValidator = attachmentsValidator;
    }

    public IEnumerable<string> Validate(AttachmentsWrapper<SendEmailRequest> request)
    {
        foreach (var error in ValidateEmail(request.Email))
            yield return error;

        if (request.Attachments is not null)
        {
            foreach (var error in attachmentsValidator.Validate(request.Attachments))
                yield return error;
        }
    }

    public IEnumerable<string> Validate(FormFilesWrapper<SendEmailRequest> request)
    {
        foreach (var error in ValidateEmail(request.Email))
            yield return error;

        if (request.Attachments is not null)
        {
            foreach (var error in formFileAttachmentsValidator.Validate(request.Attachments))
                yield return error;
        }
    }

    private IEnumerable<string> ValidateEmail(SendEmailRequest request)
    {
        var errors = baseEmailRequestValidator.Validate(request);
        foreach (var error in errors)
            yield return error;

        if (request.Subject.IsNullOrWhiteSpace() || request.Subject is { Length: > 256 })
            yield return "Invalid subject. Subject should be not empty and have length less than 256 characters";

        if (request.Body.IsNullOrWhiteSpace() || request.Body is { Length: > 8192 })
            yield return "Invalid subject. Subject should be not empty and have length less than 8192 characters";
    }
}