using ArchitectProg.FunctionalExtensions.Extensions;
using ArchitectProg.FunctionalExtensions.Services.Interfaces;
using Microservice.Email.Infrastructure.Messaging.Interfaces;
using Microservice.Email.Infrastructure.Messaging.Messages;
using RabbitMQ.Client;

namespace Microservice.Email.Infrastructure.Messaging;

public sealed class BusPublisher : IBusPublisher
{
    private readonly IChannelProvider channelFactory;
    private readonly IJsonSerializer jsonSerializer;

    public BusPublisher(IChannelProvider channelFactory, IJsonSerializer jsonSerializer)
    {
        this.channelFactory = channelFactory;
        this.jsonSerializer = jsonSerializer;
    }

    public Task Publish<T>(BusMessage<T> message)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(nameof(message.Exchange));

        var body = jsonSerializer.Serialize(message.Payload).ToBytes();

        var channel = channelFactory.Channel;
        channel.BasicPublish(message.Exchange, string.Empty, body: body);

        return Task.CompletedTask;
    }
}