namespace Microservice.Email.Core.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class CounterMetricAttribute : Attribute
{
    public string MetricName { get; }

    public CounterMetricAttribute(string metricName)
    {
        MetricName = metricName;
    }
}
