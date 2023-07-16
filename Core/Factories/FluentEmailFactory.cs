using System.Dynamic;
using ArchitectProg.FunctionalExtensions.Services.Interfaces;
using FluentEmail.Core;
using Microservice.Email.Contracts.Requests;
using Microservice.Email.Core.Factories.Interfaces;
using Newtonsoft.Json;
using Scriban;
using IFluentEmailFactory = Microservice.Email.Core.Factories.Interfaces.IFluentEmailFactory;

namespace Microservice.Email.Core.Factories;

public class FluentEmailFactory : IFluentEmailFactory
{
    private readonly IAssemblyFileReader assemblyFileReader;
    private readonly IAddressFactory addressFactory;
    private readonly IFluentEmail fluentEmail;

    public FluentEmailFactory(
        IAssemblyFileReader assemblyFileReader,
        IAddressFactory addressFactory,
        IFluentEmail fluentEmail)
    {
        this.assemblyFileReader = assemblyFileReader;
        this.addressFactory = addressFactory;
        this.fluentEmail = fluentEmail;
    }

    public IFluentEmail GetEmail(SendEmailRequest request)
    {
        var recipients = request.Recipients
            .Select(addressFactory.CreateAddress)
            .ToArray();

        var email = fluentEmail
            .To(recipients)
            .Subject(request.Subject)
            .Body(request.Body);

        if (request.Sender is not null)
            email.SetFrom(request.Sender.Email, request.Sender.Name);

        return email;
    }

    public IFluentEmail GetEmail(SendTemplatedEmailRequest request)
    {
        var recipients = request.Recipients
            .Select(addressFactory.CreateAddress)
            .ToArray();

        var properties = request.Properties.ToString();

        var model = (object)JsonConvert.DeserializeObject<ExpandoObject>(request.Properties.ToString());

        var templateContent = assemblyFileReader.GetFileFromCurrentAssembly($"./Templates/{request.TemplateName}");
        var template = Template.Parse(templateContent);

        var body = template.Render(new {model}, member => member.Name.ToLower());

        var email = fluentEmail
            .To(recipients)
            .Subject("sss")
            .Body(body, true);

        if (request.Sender is not null)
            email.SetFrom(request.Sender.Email, request.Sender.Name);

        return email;
    }
}