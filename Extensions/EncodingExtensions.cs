using System.Text;

namespace Microservice.Email.Extensions;

public static class EncodingExtensions
{
    public static ReadOnlyMemory<byte> ToBytes(this string source)
    {
        var result = Encoding.UTF8.GetBytes(source);
        return result;
    }

    public static string FromBytes(this ReadOnlyMemory<byte> source)
    {
        var result = Encoding.UTF8.GetString(source.ToArray());
        return result;
    }
}
