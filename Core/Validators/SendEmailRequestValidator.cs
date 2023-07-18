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

    public IList<string> Validate(SendEmailRequest baseEmailRequest)
    {
        var result = baseEmailRequestValidator.Validate(baseEmailRequest);
        return result;
    }
}
