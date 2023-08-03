using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microservice.Email.Infrastructure.Messaging;
using Microservice.Email.Infrastructure.Messaging.Factories;
using Microservice.Email.Infrastructure.Messaging.Factories.Interfaces;
using Microservice.Email.Infrastructure.Messaging.Interfaces;
using Microservice.Email.Infrastructure.Messaging.Settings;

namespace Microservice.Email.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddRabbitMQBusMessage(
        this IServiceCollection serviceCollection,
        Action<IMessageBusSettingsBuilder> configure)
    {
        var builder = new MessageBusSettingsBuilder();
        configure.Invoke(builder);
        var busSettings = builder.Build();

        serviceCollection.AddScopedHandlers(busSettings);

        serviceCollection.AddScoped<IChannelProvider, ChannelProvider>();
        serviceCollection.AddScoped<IBusPublisher, BusPublisher>();

        serviceCollection.AddSingleton<IExchangeFactory, ExchangeFactory>();
        serviceCollection.AddSingleton<IQueueFactory, QueueFactory>();
        serviceCollection.AddSingleton<IHandlerFactory, HandlerFactory>();
        serviceCollection.AddSingleton(busSettings);

        serviceCollection.AddHostedService<RabbitMQBusInitializer>();

        return serviceCollection;
    }

    public static void AddInterceptedScoped<TInterface, TImplementation, TInterceptor>(this IServiceCollection services)
        where TInterface : class
        where TImplementation : class, TInterface
        where TInterceptor : class, IInterceptor
    {
        services.TryAddSingleton<IProxyGenerator, ProxyGenerator>();
        services.TryAddScoped<TImplementation>();
        services.TryAddScoped<TInterceptor>();

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
