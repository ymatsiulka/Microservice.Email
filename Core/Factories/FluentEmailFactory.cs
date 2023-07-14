using FluentEmail.Core;
using Microservice.Email.Contracts.Requests;
using Microservice.Email.Core.Factories.Interfaces;
using IFluentEmailFactory = Microservice.Email.Core.Factories.Interfaces.IFluentEmailFactory;

namespace Microservice.Email.Core.Factories;

public class FluentEmailFactory : IFluentEmailFactory
{
    private readonly IAddressFactory addressFactory;
    private readonly IFluentEmail fluentEmail;

    public FluentEmailFactory(
        IAddressFactory addressFactory,
        IFluentEmail fluentEmail)
    {
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

        var email = fluentEmail
            .To(recipients);
            //.Subject(request.Subject)
            //.Body(request.Body);

        if (request.Sender is not null)
            email.SetFrom(request.Sender.Email, request.Sender.Name);

        return email;
    }
}