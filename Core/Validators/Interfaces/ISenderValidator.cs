using Microservice.Email.Contracts.Common;

namespace Microservice.Email.Core.Validators.Interfaces;

public interface ISenderValidator
{
    IEnumerable<string> Validate(Sender sender);
}