using Microservice.Email.Core.Extensions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
            v => JsonExtensions.Serialize(v),
            v => JsonExtensions.Deserialize<T>(v)
        );

        return converter;
    }

    private static ValueComparer<T?> CreateValueComparer<T>()
    {
        var comparer = new ValueComparer<T?>
        (
            (l, r) => JsonExtensions.Serialize(l) == JsonExtensions.Serialize(r),
            v => v != null ? JsonExtensions.Serialize(v).GetHashCode(StringComparison.Ordinal) : 0,
            v => JsonExtensions.Deserialize<T>(JsonExtensions.Serialize(v))
        );

        return comparer;
    }
}
