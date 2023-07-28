using RabbitMQ.Client;

namespace Microservice.Email.Infrastructure.RabbitMQ.Interfaces;

public interface IRabbitMQChannelService : IDisposable
{
    IModel GetChannel();
    void PublishMessage(string exchange, string queue, ReadOnlyMemory<byte> body);
}
