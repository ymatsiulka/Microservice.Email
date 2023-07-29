namespace Microservice.Email.Infrastructure.RabbitMQ.Interfaces;

public interface IMessageBusSettingsBuilder
{
    IMessageBusSettingsBuilder RegisterExchange(string name);
    IMessageBusSettingsBuilder RegisterHandler<THandlerType>(string queueName) where THandlerType : IRabbitMQMessageHandler;
    BusSettings Build();
}
