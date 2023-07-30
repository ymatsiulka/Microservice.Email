using App.Metrics;
using App.Metrics.Counter;
using Castle.DynamicProxy;
using Microservice.Email.Core.Attributes;

namespace Microservice.Email.Interceptors.Metrics;

public sealed class CounterMetricInterceptor : IInterceptor
{
    private readonly IMetrics metrics;

    public CounterMetricInterceptor(IMetrics metrics)
    {
        this.metrics = metrics;
    }

    public void Intercept(IInvocation invocation)
    {
        var method = invocation.MethodInvocationTarget ?? invocation.Method;
        var metricAttribute = method.GetCustomAttributes(true).OfType<CounterMetricAttribute>().FirstOrDefault();
        if (metricAttribute != null)
        {
            invocation.Proceed();
            var counterOptions = new CounterOptions
            {
                Name = metricAttribute.MetricName,
                Context = "EmailApi",
                MeasurementUnit = Unit.Calls
            };
            metrics.Measure.Counter.Increment(counterOptions);
        }
    }
}
