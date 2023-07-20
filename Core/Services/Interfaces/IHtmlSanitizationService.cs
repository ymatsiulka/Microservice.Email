namespace Microservice.Email.Core.Services.Interfaces;

public interface IHtmlSanitizationService
{
    string Sanitize(string html);
}
