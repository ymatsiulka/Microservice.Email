using System.Dynamic;
using ArchitectProg.FunctionalExtensions.Services.Interfaces;
using Microservice.Email.Contracts.Requests;
using Microservice.Email.Core.Exceptions;
using Microservice.Email.Core.Services.Interfaces;
using Newtonsoft.Json;
using Scriban;

namespace Microservice.Email.Core.Services;

public class TemplatedEmailService : ITemplatedEmailService
{
    private const string TitleOpenTag = "<title>";
    private const string TitleCloseTag = "</title>";
    private const string TemplateDirectoryPath = "./Templates/{0}.html";

    private readonly IAssemblyFileReader assemblyFileReader;

    public TemplatedEmailService(IAssemblyFileReader assemblyFileReader)
    {
        this.assemblyFileReader = assemblyFileReader;
    }

    public async Task<SendEmailRequest> ProcessTemplatedRequest(SendTemplatedEmailRequest request)
    {
        var templatePath = string.Format(TemplateDirectoryPath, request.TemplateName);
        var templateText = assemblyFileReader.GetFileFromCurrentAssembly(templatePath);

        var template = Template.Parse(templateText);
        if (template.HasErrors)
        {
            var errors = template.Messages.ToString();
            throw new InvalidTemplateException(templatePath, errors);
        }

        var properties = request.TemplateProperties?.ToString() ?? string.Empty;
        var model = JsonConvert.DeserializeObject<ExpandoObject>(properties);

        var body = await template.RenderAsync(new { model }, property => property.Name.ToLower());
        var subject = GetSubject(body);

        var result = new SendEmailRequest
        {
            Sender = request.Sender,
            Recipients = request.Recipients,
            Attachments = request.Attachments,
            Subject = subject,
            Body = body
        };

        return result;
    }

    private static string GetSubject(string body)
    {
        var titleStart = body.IndexOf(TitleOpenTag, StringComparison.Ordinal);
        if (titleStart < 0)
            return string.Empty;

        var titleEndIndex = body.IndexOf(TitleCloseTag, StringComparison.Ordinal);
        if (titleEndIndex < 0)
            return string.Empty;

        var subjectStart = titleStart + TitleOpenTag.Length;
        var subjectLength = titleEndIndex - subjectStart;

        var result = body.Substring(subjectStart, subjectLength);
        return result;
    }
}