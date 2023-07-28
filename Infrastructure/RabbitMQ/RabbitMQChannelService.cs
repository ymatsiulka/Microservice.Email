using ArchitectProg.FunctionalExtensions.Extensions;
using Microservice.Email.Infrastructure.RabbitMQ.Interfaces;
using RabbitMQ.Client;

namespace Microservice.Email.Infrastructure.RabbitMQ;

public sealed class RabbitMQChannelService : IRabbitMQChannelService
{
    private readonly IConnection connection;

    private readonly IModel channel;

    public RabbitMQChannelService(IRabbitMQConnectionFactory rabbitMQConnectionFactory)
    {
        connection = rabbitMQConnectionFactory.CreateConnection();
        channel = connection.CreateModel();
    }

    public IModel GetChannel() => channel;

    public void PublishMessage(string exchange, string queue, ReadOnlyMemory<byte> body)
    {
        if (exchange.IsNullOrWhiteSpace())
            throw new ArgumentNullException(nameof(exchange));

        if (queue.IsNullOrWhiteSpace())
            throw new ArgumentNullException(nameof(exchange));

        channel.BasicPublish(exchange, queue, body: body);
    }

    public void Dispose()
    {
        if (channel != null)
        {
            if (channel.IsOpen)
            {
                channel.Close();
            }

            channel.Dispose();
        }

        if (connection != null)
        {
            if (connection.IsOpen)
            {
                connection.Close();
            }

            connection.Dispose();
        }
    }


}
