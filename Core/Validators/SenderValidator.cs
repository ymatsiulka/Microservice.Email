using Microservice.Email.Core.Contracts.Common;
using Microservice.Email.Core.Validators.Interfaces;
using Microservice.Email.Extensions;

namespace Microservice.Email.Core.Validators;

public sealed class SenderValidator : ISenderValidator
{
    public IEnumerable<string> Validate(Sender sender)
    {
        if (!sender.Email.IsValidEmail())
            yield return "Invalid sender email";

        if (sender.Name is { Length: > 128 })
            yield return "Too long sender name. Sender name must be less than 128 characters";
    }
}