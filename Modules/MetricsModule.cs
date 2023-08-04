using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using Microservice.Email.Modules.Interfaces;

namespace Microservice.Email.Modules;

public sealed class MetricsModule : IModule
{
    public void RegisterDependencies(WebApplicationBuilder builder)
    {
        builder.Host.UseMetricsWebTracking();
        builder.Host.UseMetrics(options =>
        {
            options.EndpointOptions = endpointOptions =>
            {
                endpointOptions.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
                endpointOptions.MetricsEndpointOutputFormatter = new MetricsPrometheusProtobufOutputFormatter();
                endpointOptions.EnvironmentInfoEndpointEnabled = false;
            };
        });

        builder.Services.AddMetrics();
        builder.Services.AddMetricsTrackingMiddleware();
    }
}
