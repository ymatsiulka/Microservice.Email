using Microservice.Email.Contracts.Requests.Base;

namespace Microservice.Email.Core.Validators.Interfaces;

public interface IBaseEmailRequestValidator
{
    IEnumerable<string> Validate(BaseEmailRequest request);
}
