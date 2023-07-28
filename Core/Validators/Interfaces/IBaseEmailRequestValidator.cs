using Microservice.Email.Core.Contracts.Requests.Base;

namespace Microservice.Email.Core.Validators.Interfaces;

public interface IBaseEmailRequestValidator
{
    IEnumerable<string> Validate(BaseEmailRequest request);
}
