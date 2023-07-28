using Microservice.Email.Infrastructure.RabbitMQ.Interfaces;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Microservice.Email.Infrastructure.RabbitMQ;

public sealed class RabbitMQConnectionFactory : IRabbitMQConnectionFactory
{
    private readonly RabbitMQSettings rabbitMQSettings;
    
    public RabbitMQConnectionFactory(IOptions<RabbitMQSettings> rabbitMQSettings)
    {
        this.rabbitMQSettings = rabbitMQSettings.Value;
    }

    public IConnection CreateConnection()
    {
        var connectionFactory = new ConnectionFactory()
        {
            HostName = rabbitMQSettings.Host,
            Port = rabbitMQSettings.Port,
            UserName = rabbitMQSettings.Username,
            Password = rabbitMQSettings.Password,
            DispatchConsumersAsync = true
        };

        var result = connectionFactory.CreateConnection();
        return result;
    }
}
