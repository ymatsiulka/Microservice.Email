using Ganss.Xss;
using Microservice.Email.Infrastructure.Smtp.Interfaces;

namespace Microservice.Email.Infrastructure.Smtp;

public sealed class SanitizationService : ISanitizationService
{
    private readonly HtmlSanitizer sanitizer;

    public SanitizationService()
    {
        sanitizer = new HtmlSanitizer();
        sanitizer.AllowedAttributes.Add("class");
    }

    public string Sanitize(string html)
    {
        var result = sanitizer.Sanitize(html);
        return result;
    }
}
