namespace Microservice.Email.Infrastructure.RabbitMQ.Interfaces;

public interface IMessageBusSettingsBuilder
{
    IMessageBusSettingsBuilder RegisterExchange(string name);
    IMessageBusSettingsBuilder RegisterHandler<THandlerType, THandlerImplementationType, TPayload>(string queueName)
        where TPayload : class
        where THandlerImplementationType : class
        where THandlerType : IRabbitMQMessageHandler<TPayload>;

    BusSettings Build();
}
