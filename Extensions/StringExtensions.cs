using ArchitectProg.FunctionalExtensions.Extensions;
using System.Text;

namespace Microservice.Email.Extensions;

public static class StringExtensions
{
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
