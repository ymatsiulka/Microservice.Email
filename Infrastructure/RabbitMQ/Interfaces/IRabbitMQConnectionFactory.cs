using RabbitMQ.Client;

namespace Microservice.Email.Infrastructure.RabbitMQ.Interfaces;

public interface IRabbitMQConnectionFactory
{
    IConnection CreateConnection();
}
