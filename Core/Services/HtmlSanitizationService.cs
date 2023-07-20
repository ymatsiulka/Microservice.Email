using Ganss.Xss;
using Microservice.Email.Core.Services.Interfaces;

namespace Microservice.Email.Core.Services;

public class HtmlSanitizationService : IHtmlSanitizationService
{
    private readonly HtmlSanitizer sanitizer;

    public HtmlSanitizationService()
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
