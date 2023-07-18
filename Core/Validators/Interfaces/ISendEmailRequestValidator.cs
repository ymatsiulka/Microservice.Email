using Microservice.Email.Contracts.Requests;

namespace Microservice.Email.Core.Validators.Interfaces;

public interface ISendEmailRequestValidator
{
    IList<string> Validate(SendEmailRequest baseEmailRequest);
}
