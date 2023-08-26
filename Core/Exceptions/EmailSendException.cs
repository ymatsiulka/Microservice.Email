using ArchitectProg.FunctionalExtensions.Constants;

namespace Microservice.Email.Core.Exceptions;

public sealed class EmailSendException : Exception
{
    public EmailSendException(IEnumerable<string> errorMessages)
        : base(string.Join(GenericConstants.Space, errorMessages))
    {
    }

    public EmailSendException(IEnumerable<string> errorMessages, Exception inner)
        : base(string.Join(GenericConstants.Space, errorMessages), inner)
    {
    }
}