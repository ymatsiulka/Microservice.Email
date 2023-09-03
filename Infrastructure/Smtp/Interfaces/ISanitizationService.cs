namespace Microservice.Email.Infrastructure.Smtp.Interfaces;

public interface ISanitizationService
{
    string Sanitize(string html);
}
