using System.Text;
using ArchitectProg.FunctionalExtensions.Extensions;

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

    public static string StripUnicodeCharacters(this string? source)
    {
        if (source.IsNullOrWhiteSpace())
            return string.Empty;

        var builder = new StringBuilder();
        builder
            .Append(source.Normalize(NormalizationForm.FormKD).Where(x => x < 128).ToArray())
            .Replace("\r", string.Empty)
            .Replace("\n", string.Empty);

        return builder.ToString();
    }
}
