using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace Microservice.Email.Extensions;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<T> SetPropertyConverterAndComparer<T>(this PropertyBuilder<T> propertyBuilder)
    {
        var converter = CreateValueConverter<T>();
        var comparer = CreateValueComparer<T>();

        propertyBuilder
            .HasConversion(converter)
            .Metadata
            .SetValueConverter(converter);

        propertyBuilder
            .Metadata.SetValueComparer(comparer);

        return propertyBuilder;
    }

    private static ValueConverter<T, string> CreateValueConverter<T>()
    {
        var converter = new ValueConverter<T, string>(
            v => JsonConvert.SerializeObject(v),
            v => JsonConvert.DeserializeObject<T>(v)
        );

        return converter;
    }

    private static ValueComparer<T?> CreateValueComparer<T>()
    {
        var comparer = new ValueComparer<T?>
        (
            (l, r) => JsonConvert.SerializeObject(l) == JsonConvert.SerializeObject(r),
            v => v != null ? JsonConvert.SerializeObject(v).GetHashCode(StringComparison.Ordinal) : 0,
            v => JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(v))
        );

        return comparer;
    }
}
