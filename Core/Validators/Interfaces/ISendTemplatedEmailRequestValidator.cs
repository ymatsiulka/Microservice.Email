using Microservice.Email.Contracts.Requests;

namespace Microservice.Email.Core.Validators.Interfaces;

public interface ISendTemplatedEmailRequestValidator
{
    IList<string> Validate(SendTemplatedEmailRequest sendTemplateEmailRequest);
}
