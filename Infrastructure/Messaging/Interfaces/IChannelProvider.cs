using RabbitMQ.Client;

namespace Microservice.Email.Infrastructure.Messaging.Interfaces;

public interface IChannelProvider : IAsyncDisposable
{
    IChannel Channel { get; }

    Task<IChannel> CreateChannelAsync();
}