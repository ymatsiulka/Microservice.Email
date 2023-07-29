using Microservice.Email.Core.Contracts.Responses;
using Microservice.Email.Infrastructure.RabbitMQ.Messages.Base;

namespace Microservice.Email.Infrastructure.RabbitMQ.Messages;

public class EmailSentMessage : RabbitMQMessage<EmailResponse>
{
}
