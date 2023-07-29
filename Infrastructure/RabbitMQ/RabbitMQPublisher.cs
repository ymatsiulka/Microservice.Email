using Microservice.Email.Extensions;
using Microservice.Email.Infrastructure.RabbitMQ.Interfaces;
using Microservice.Email.Infrastructure.RabbitMQ.Messages.Base;

namespace Microservice.Email.Infrastructure.RabbitMQ;

public sealed class RabbitMQPublisher : IRabbitMQPublisher
{
    private readonly IRabbitMQChannelService rabbitMQChannelService;

    public RabbitMQPublisher(IRabbitMQChannelService rabbitMQChannelService)
    {
        this.rabbitMQChannelService = rabbitMQChannelService;

    }
    public Task Publish<T>(RabbitMQMessage<T> message)
    {
        var body = message.Payload.Serialize().ToBytes();

        rabbitMQChannelService.PublishMessage(message.Exchange, message.Queue, body);

        return Task.CompletedTask;
    }
}
