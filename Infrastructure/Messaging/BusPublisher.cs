using ArchitectProg.FunctionalExtensions.Extensions;
using Microservice.Email.Extensions;
using Microservice.Email.Infrastructure.Messaging.Interfaces;
using Microservice.Email.Infrastructure.Messaging.Messages;
using RabbitMQ.Client;

namespace Microservice.Email.Infrastructure.Messaging;

public sealed class BusPublisher : IBusPublisher
{
    private readonly IChannelProvider channelFactory;

    public BusPublisher(IChannelProvider channelFactory)
    {
        this.channelFactory = channelFactory;
    }

    public Task Publish<T>(BusMessage<T> message)
    {
        if (message.Exchange.IsNullOrWhiteSpace())
            throw new ArgumentNullException(nameof(message.Exchange));

        var body = message.Payload.Serialize().ToBytes();

        var channel = channelFactory.Channel;
        channel.BasicPublish(message.Exchange, string.Empty);

        return Task.CompletedTask;
    }
}