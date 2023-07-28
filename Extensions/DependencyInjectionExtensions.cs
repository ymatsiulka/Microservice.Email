using Microservice.Email.Infrastructure.RabbitMQ;
using Microservice.Email.Infrastructure.RabbitMQ.Interfaces;

namespace Microservice.Email.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddRabbitMQ(this IServiceCollection serviceCollection)
    {
        if (serviceCollection == null)
        {
            throw new ArgumentNullException(nameof(serviceCollection));
        }

        serviceCollection.AddScoped<IRabbitMQChannelService, RabbitMQChannelService>();
        serviceCollection.AddScoped<IRabbitMQConnectionFactory, RabbitMQConnectionFactory>();
        return serviceCollection;
    }
}
