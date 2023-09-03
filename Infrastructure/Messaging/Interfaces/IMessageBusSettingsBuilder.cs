using Microservice.Email.Infrastructure.Messaging.Settings;

namespace Microservice.Email.Infrastructure.Messaging.Interfaces;

public interface IMessageBusSettingsBuilder
{
    IMessageBusSettingsBuilder RegisterExchange(string name);
    IMessageBusSettingsBuilder RegisterHandler<THandler, TImplementation>(
        string queueName,
        Action<QueueProperties>? configureProperties = null)
        where THandler : IMessageHandler
        where TImplementation : IMessageHandler, THandler;

    BusSettings Build();
}
