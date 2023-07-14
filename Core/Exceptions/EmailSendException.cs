namespace Microservice.Email.Core.Exceptions;

public class EmailSendException : Exception
{
    public EmailSendException()
    {
    }

    public EmailSendException(string message) : base(message)
    {
    }

    public EmailSendException(string message, Exception inner) : base(message, inner)
    {
    }
}