using System.Reflection;

namespace Microservice.Email.Extensions;

public static class ReflectionExtensions
{
    public static T[] FindImplementationsOfTypeInExecutingAssembly<T>() where T : class
    {
        var interfaceType = typeof(T);
        var currentAssembly = Assembly.GetExecutingAssembly();

        T[] implementations = currentAssembly.GetTypes()
            .Where(type => interfaceType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            .Select(Activator.CreateInstance)
            .OfType<T>()
            .ToArray();

        return implementations;
    }
}
