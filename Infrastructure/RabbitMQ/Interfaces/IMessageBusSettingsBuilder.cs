namespace Microservice.Email.Infrastructure.RabbitMQ.Interfaces;

public interface IMessageBusSettingsBuilder
{
    IMessageBusSettingsBuilder RegisterExchange(string name);
    IMessageBusSettingsBuilder RegisterHandler<THandlerType, TPayload>(string queueName)
        where TPayload : class
        where THandlerType : IRabbitMQMessageHandler<TPayload>;

    BusSettings Build();
}
