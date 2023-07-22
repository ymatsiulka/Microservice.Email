using Microservice.Email.Contracts.Requests;

namespace Microservice.Email.Core.Validators.Interfaces;

public interface ISendEmailRequestValidator
{
    IEnumerable<string> Validate(SendEmailRequest request);
}
