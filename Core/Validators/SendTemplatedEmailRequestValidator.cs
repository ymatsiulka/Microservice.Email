using Microservice.Email.Contracts.Requests;
using Microservice.Email.Core.Validators.Interfaces;

namespace Microservice.Email.Core.Validators;

public sealed class SendTemplatedEmailRequestValidator : ISendTemplatedEmailRequestValidator
{
    private readonly IBaseEmailRequestValidator baseEmailRequestValidator;

    public SendTemplatedEmailRequestValidator(IBaseEmailRequestValidator baseEmailRequestValidator)
    {
        this.baseEmailRequestValidator = baseEmailRequestValidator;
    }

    public IList<string> Validate(SendTemplatedEmailRequest sendTemplateEmailRequest)
    {
        var result = baseEmailRequestValidator.Validate(sendTemplateEmailRequest);
        return result;
    }
}
