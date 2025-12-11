using Microservice.Email.Infrastructure.Messaging.Interfaces;
using Microservice.Email.Infrastructure.Messaging.Settings;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Microservice.Email.Infrastructure.Messaging;

public sealed class ChannelProvider : IChannelProvider
{
    private readonly MessagingSettings messagingSettings;
    private readonly List<IChannel> channels = [];
    
    private IConnection? connection;
    private IChannel? channel;

    public IChannel Channel => channel ?? throw new InvalidOperationException("Channel not initialized.");

    public ChannelProvider(IOptions<MessagingSettings> messagingSettings)
    {
        this.messagingSettings = messagingSettings.Value;
    }

    private async Task<IConnection> GetOrCreateConnectionAsync()
    {
        if (connection is not null)
            return connection;

        var connectionFactory = new ConnectionFactory
        {
            HostName = messagingSettings.Host,
            Port = messagingSettings.Port,
            UserName = messagingSettings.Username,
            Password = messagingSettings.Password,
            VirtualHost = "/"
        };

        connection = await connectionFactory.CreateConnectionAsync();
        return connection;
    }

    public async Task<IChannel> CreateChannelAsync()
    {
        var conn = await GetOrCreateConnectionAsync();
        var newChannel = await conn.CreateChannelAsync();
        await newChannel.BasicQosAsync(0, 1, false);

        channels.Add(newChannel);
        channel ??= newChannel;

        return newChannel;
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var c in channels)
        {
            if (c.IsOpen)
                await c.CloseAsync();

            await c.DisposeAsync();
        }

        if (connection is not null)
        {
            if (connection.IsOpen)
                await connection.CloseAsync();

            await connection.DisposeAsync();
        }
    }
}
