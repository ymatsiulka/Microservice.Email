using System.Text.Json;

namespace Microservice.Email.Extensions;

public static class JsonExtensions
{
    private static readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static string Serialize<T>(this T source)
    {
        var result = JsonSerializer.Serialize(source, jsonSerializerOptions);
        return result;
    }

    public static T DeserializeOrThrow<T>(this string source)
    {
        var result = JsonSerializer.Deserialize<T>(source, jsonSerializerOptions)
            ?? throw new InvalidOperationException("Deserialization result can't be null");
        
        return result;
    }
}
