using ArchitectProg.FunctionalExtensions.Extensions;
using Microservice.Email.Contracts.Requests;
using Microservice.Email.Core.Validators.Interfaces;

namespace Microservice.Email.Core.Validators;

public sealed class SendEmailRequestValidator : ISendEmailRequestValidator
{
    private readonly IBaseEmailRequestValidator baseEmailRequestValidator;

    public SendEmailRequestValidator(IBaseEmailRequestValidator baseEmailRequestValidator)
    {
        this.baseEmailRequestValidator = baseEmailRequestValidator;
    }

    public IEnumerable<string> Validate(SendEmailRequest request)
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