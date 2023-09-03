using Microservice.Email.Core.Contracts.Requests;
using Microservice.Email.Core.Validators.Interfaces;

namespace Microservice.Email.Core.Validators;

public sealed class SendTemplatedEmailRequestValidator : ISendTemplatedEmailRequestValidator
{
    private readonly IBaseEmailRequestValidator baseEmailRequestValidator;
    private readonly IFormFileAttachmentsValidator formFileAttachmentsValidator;
    private readonly IAttachmentsValidator attachmentsValidator;

    public SendTemplatedEmailRequestValidator(
        IBaseEmailRequestValidator baseEmailRequestValidator,
        IFormFileAttachmentsValidator formFileAttachmentsValidator,
        IAttachmentsValidator attachmentsValidator)
    {
        this.baseEmailRequestValidator = baseEmailRequestValidator;
        this.formFileAttachmentsValidator = formFileAttachmentsValidator;
        this.attachmentsValidator = attachmentsValidator;
    }

    public IEnumerable<string> Validate(AttachmentsWrapper<SendTemplatedEmailRequest> request)
    {
        foreach (var error in ValidateEmail(request.Email))
            yield return error;

        if (request.Attachments is not null)
        {
            foreach (var error in attachmentsValidator.Validate(request.Attachments))
                yield return error;
        }
    }

    public IEnumerable<string> Validate(FormFilesWrapper<SendTemplatedEmailRequest> request)
    {
        foreach (var error in ValidateEmail(request.Email))
            yield return error;

        if (request.Attachments is not null)
        {
            foreach (var error in formFileAttachmentsValidator.Validate(request.Attachments))
                yield return error;
        }
    }

    private IEnumerable<string> ValidateEmail(SendTemplatedEmailRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        var errors = baseEmailRequestValidator.Validate(request);
        foreach (var error in errors)
            yield return error;

        if (request.TemplateProperties is { Length: > 8192 })
            yield return "Invalid template properties. Properties length should be less than 8192 characters";
    }
}