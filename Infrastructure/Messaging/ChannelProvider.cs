using Microservice.Email.Infrastructure.Messaging.Interfaces;
using Microservice.Email.Infrastructure.Messaging.Settings;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Microservice.Email.Infrastructure.Messaging;

public sealed class ChannelProvider : IChannelProvider
{
    private readonly RabbitMQSettings rabbitMQSettings;
    private readonly Lazy<IConnection> connection;
    private readonly Lazy<IModel> channel;
    private readonly List<IModel> channels = new();

    public IModel Channel => channel.Value;

    public ChannelProvider(IOptions<RabbitMQSettings> rabbitMQSettings)
    {
        this.rabbitMQSettings = rabbitMQSettings.Value;

        connection = new Lazy<IConnection>(GetConnection, LazyThreadSafetyMode.ExecutionAndPublication);
        channel = new Lazy<IModel>(CreateChannel, LazyThreadSafetyMode.ExecutionAndPublication);
    }

    private IConnection GetConnection()
    {
        var connectionFactory = new ConnectionFactory
        {
            HostName = rabbitMQSettings.Host,
            Port = rabbitMQSettings.Port,
            UserName = rabbitMQSettings.Username,
            Password = rabbitMQSettings.Password,
            DispatchConsumersAsync = true,
            VirtualHost = "/"
        };

        var result = connectionFactory.CreateConnection();
        return result;
    }

    public IModel CreateChannel()
    {
        var result = connection.Value.CreateModel();
        result.BasicQos(0, 1, false);

        channels.Add(result);

        return result;
    }

    public void Dispose()
    {
        foreach (var c in channels)
        {
            if (c.IsOpen)
                c.Close();

            c.Dispose();
        }

        if (connection.IsValueCreated)
        {
            if (connection.Value.IsOpen)
                connection.Value.Close();

            connection.Value.Dispose();
        }
    }
}
