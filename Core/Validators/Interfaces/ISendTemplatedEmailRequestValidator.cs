using Microservice.Email.Core.Contracts.Requests;

namespace Microservice.Email.Core.Validators.Interfaces;

public interface ISendTemplatedEmailRequestValidator
{
    IEnumerable<string> Validate(SendTemplatedEmailRequest request);
}
