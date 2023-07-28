using Microservice.Email.Core.Contracts.Requests;
using Microservice.Email.Core.Validators.Interfaces;

namespace Microservice.Email.Core.Validators;

public sealed class SendTemplatedEmailRequestValidator : ISendTemplatedEmailRequestValidator
{
    private readonly IBaseEmailRequestValidator baseEmailRequestValidator;

    public SendTemplatedEmailRequestValidator(IBaseEmailRequestValidator baseEmailRequestValidator)
    {
        this.baseEmailRequestValidator = baseEmailRequestValidator;
    }

    public IEnumerable<string> Validate(SendTemplatedEmailRequest request)
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
