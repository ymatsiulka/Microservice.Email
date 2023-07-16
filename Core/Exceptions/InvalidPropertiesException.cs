namespace Microservice.Email.Core.Exceptions;

public class InvalidPropertiesException : Exception
{
    private const string ErrorMessage =
        "Attempting to use an invalid template. TemplatePath: {0}. Errors: {1}";

    public InvalidPropertiesException(string templatePath, string errors)
        : base(string.Format(ErrorMessage, templatePath, errors))
    {
    }

    public InvalidPropertiesException(string templatePath, string errors, Exception inner)
        : base(string.Format(ErrorMessage, templatePath, errors), inner)
    {
    }
}