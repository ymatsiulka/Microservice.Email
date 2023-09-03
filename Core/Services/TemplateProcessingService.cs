using ArchitectProg.FunctionalExtensions.Services.Interfaces;
using Microservice.Email.Core.Exceptions;
using Microservice.Email.Core.Services.Interfaces;
using Scriban;
using System.Dynamic;
using Microservice.Email.Core.Contracts.Common;
using Microservice.Email.Core.Contracts.Requests;
using Newtonsoft.Json;

namespace Microservice.Email.Core.Services;

public sealed class TemplateProcessingService : ITemplateProcessingService
{
    private const string TitleOpenTag = "<title>";
    private const string TitleCloseTag = "</title>";
    private const string TemplateDirectoryPath = "./Templates/{0}.html";

    private readonly IAssemblyFileReader assemblyFileReader;
    private readonly IJsonSerializer jsonSerializer;

    public TemplateProcessingService(IAssemblyFileReader assemblyFileReader, IJsonSerializer jsonSerializer)
    {
        this.assemblyFileReader = assemblyFileReader;
        this.jsonSerializer = jsonSerializer;
    }

    public async Task<EmailContent> Process(SendTemplatedEmailRequest request)
    {
        var templatePath = string.Format(TemplateDirectoryPath, request.TemplateName);
        var templateText = assemblyFileReader.GetFileFromCurrentAssembly(templatePath);

        var template = Template.Parse(templateText);
        if (template.HasErrors)
        {
            var errors = template.Messages.ToString();
            throw new InvalidTemplateException(templatePath, errors);
        }

        var properties = request.TemplateProperties ?? string.Empty;
        var model = JsonConvert.DeserializeObject<ExpandoObject>(properties);

        var body = await template.RenderAsync(new { model }, property => property.Name.ToLower());
        var subject = GetSubject(body);

        var result = new EmailContent
        {
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