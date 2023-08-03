using Microservice.Email.Infrastructure.Messaging.Messages;

namespace Microservice.Email.Infrastructure.Messaging.Interfaces;

public interface IBusPublisher
{
    Task Publish<T>(BusMessage<T> message);
}
