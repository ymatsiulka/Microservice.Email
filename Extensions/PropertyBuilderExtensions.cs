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
            x => x.Serialize(),
            x => x.DeserializeOrThrow<T>()
        );

        return converter;
    }

    private static ValueComparer<T?> CreateValueComparer<T>()
    {
        var comparer = new ValueComparer<T?>
        (
            (x, y) => x.Serialize() == y.Serialize(),
            x => x != null ? x.Serialize().GetHashCode(StringComparison.Ordinal) : 0,
            x => x
        );

        return comparer;
    }
}
