using Castle.DynamicProxy;
using Microservice.Email.Core.Attributes;

namespace Microservice.Email.Core.Interceptors.Metrics;

public sealed class CounterMetricInterceptor : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        var method = invocation.MethodInvocationTarget ?? invocation.Method;
        var metricAttribute = method.GetCustomAttributes(true).OfType<CounterMetricAttribute>().FirstOrDefault();
        if (metricAttribute != null)
        {
            invocation.Proceed();
            if (invocation.ReturnValue is Task task && task.Exception != null)
            {
                var errorCounter = Prometheus.Metrics.CreateCounter($"{metricAttribute.Name}_error", $"{metricAttribute.Help} with errors");
                errorCounter.Inc();
                Console.WriteLine($"{metricAttribute.Name}_error");
            }
            else
            {
                var counter = Prometheus.Metrics.CreateCounter(metricAttribute.Name, metricAttribute.Help);
                counter.Inc();
            }
        }
    }
}
