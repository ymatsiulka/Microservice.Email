using RabbitMQ.Client;

namespace Microservice.Email.Infrastructure.Messaging.Interfaces;

public interface IChannelProvider : IDisposable
{
    IModel Channel { get; }

    IModel CreateChannel();
}