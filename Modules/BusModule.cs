using Microservice.Email.Core.Contracts.Requests;
using Microservice.Email.Extensions;
using Microservice.Email.Infrastructure.Messaging.Handlers;
using Microservice.Email.Infrastructure.Messaging.Interfaces;
using Microservice.Email.Infrastructure.Messaging.Settings;
using Microservice.Email.Modules.Interfaces;

namespace Microservice.Email.Modules;

public sealed class BusModule : IModule
{
    public void RegisterDependencies(WebApplicationBuilder builder)
    {
        builder.Services.AddRabbitMQBusMessage(messageBus =>
        {
            messageBus
                .RegisterExchange("email")
                .RegisterHandler<IMessageHandler<AttachmentsWrapper<SendEmailRequest>>, SendEmailMessageHandler>("sent-email-queue")
                .RegisterExchange("templated-email")
                .RegisterHandler<IMessageHandler<AttachmentsWrapper<SendTemplatedEmailRequest>>, SendTemplatedEmailMessageHandler>("sent-templated-email-queue");
        });

        var configuration = builder.Configuration;
        builder.Services.Configure<MessagingSettings>(configuration.GetSection(nameof(MessagingSettings)));
    }
}
