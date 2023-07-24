using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace Microservice.Email.Core.Extensions;

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
}
