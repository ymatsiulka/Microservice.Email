using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Microservice.Email.Extensions;

public static class JsonExtensions
{
    private static readonly JsonSerializerSettings settings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };

    public static string Serialize<T>(this T source)
    {
        var result = JsonConvert.SerializeObject(source, settings);
        return result;
    }

    public static T? Deserialize<T>(this string source)
    {
        var result = JsonConvert.DeserializeObject<T>(source, settings);
        return result;
    }

    public static T DeserializeOrThrow<T>(this string source)
    {
        var result = JsonConvert.DeserializeObject<T>(source, settings);
        if (result is null)
            throw new InvalidOperationException("Deserialization result can't be null");

        return result;
    }
}
