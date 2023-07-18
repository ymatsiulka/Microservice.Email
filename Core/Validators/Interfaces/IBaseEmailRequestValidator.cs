using Microservice.Email.Contracts.Requests.Base;

namespace Microservice.Email.Core.Validators.Interfaces;

public interface IBaseEmailRequestValidator
{
    IList<string> Validate(BaseEmailRequest baseEmailRequest);
}
