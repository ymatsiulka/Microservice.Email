using Microservice.Email.Infrastructure.RabbitMQ;
using Microservice.Email.Infrastructure.RabbitMQ.Interfaces;

namespace Microservice.Email.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddRabbitMQBusMessage(this IServiceCollection serviceCollection,
        Action<IMessageBusSettingsBuilder> configure)
    {
        if (serviceCollection == null)
        {
            throw new ArgumentNullException(nameof(serviceCollection));
        }

        serviceCollection.AddScoped<IRabbitMQChannelService, RabbitMQChannelService>();
        serviceCollection.AddScoped<IRabbitMQConnectionFactory, RabbitMQConnectionFactory>();
        serviceCollection.AddSingleton<IMessageBusSettingsBuilder, MessageBusSettingsBuilder>();
        serviceCollection.AddScoped<IRabbitMQPublisher, RabbitMQPublisher>();

        serviceCollection.AddSingleton<BusSettings>(x =>
        {
            var builder = x.GetRequiredService<IMessageBusSettingsBuilder>();
            configure.Invoke(builder);
            
            var settings = builder.Build();
            return settings;
        });

        serviceCollection.AddHostedService<RabbitMQBusInitializer>();

        return serviceCollection;
    }

    public static IServiceCollection AddMessageHandler<TMessageHandler>(this IServiceCollection serviceCollection)
    where TMessageHandler : class, IRabbitMQMessageHandler
    {
        if (serviceCollection is null)
            throw new ArgumentNullException(nameof(serviceCollection));

        serviceCollection.AddScoped<TMessageHandler>();

        return serviceCollection;
    }
}
