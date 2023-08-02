using Castle.DynamicProxy;
using Microservice.Email.Infrastructure.RabbitMQ;
using Microservice.Email.Infrastructure.RabbitMQ.Interfaces;
using Microsoft.Extensions.DependencyInjection.Extensions;

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

        var builder = new MessageBusSettingsBuilder();
        configure.Invoke(builder);
        var busSettings = builder.Build();

        serviceCollection.AddSingleton(busSettings);
        serviceCollection.AddScopedHandlers(busSettings);
        serviceCollection.AddScoped<IRabbitMQChannelService, RabbitMQChannelService>();
        serviceCollection.AddScoped<IRabbitMQConnectionFactory, RabbitMQConnectionFactory>();
        serviceCollection.AddScoped<IRabbitMQPublisher, RabbitMQPublisher>();

        serviceCollection.AddHostedService<RabbitMQBusInitializer>();

        return serviceCollection;
    }

    public static void AddInterceptorSingleton<TInterface, TImplementation, TInterceptor>(this IServiceCollection services)
        where TInterface : class
        where TImplementation : class, TInterface
        where TInterceptor : class, IInterceptor
    {
        services.TryAddSingleton<IProxyGenerator, ProxyGenerator>();
        services.TryAddScoped<TImplementation>();
        services.TryAddTransient<TInterceptor>();

        services.AddScoped(provider =>
        {
            var proxyGenerator = provider.GetRequiredService<IProxyGenerator>();
            var impl = provider.GetRequiredService<TImplementation>();
            var interceptor = provider.GetRequiredService<TInterceptor>();
            return proxyGenerator.CreateInterfaceProxyWithTarget<TInterface>(impl, interceptor);
        });
    }

    private static void AddScopedHandlers(this IServiceCollection serviceCollection, BusSettings busSettings)
    {
        foreach (var exchange in busSettings.Exchanges)
            foreach (var queue in exchange.Queues)
                serviceCollection.AddScoped(queue.HandlerType, queue.HandlerImplementationType);
    }
}
