namespace Microservice.Email.Core.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class CounterMetricAttribute : Attribute
{
    public string Name { get; }
    public string Help { get; }

    public CounterMetricAttribute(string name, string help)
    {
        Name = name;
        Help = help;
    }
}
