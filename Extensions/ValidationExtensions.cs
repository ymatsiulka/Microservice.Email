namespace Microservice.Email.Extensions;

public static class ValidationExtensions
{
    public static bool IsValidEmail(this string email)
    {
        if (email.Split('@').Length != 2)
        {
            return false;
        }

        var parts = email.Split('@');
        var localPart = parts[0];
        var domainPart = parts[1];

        if (string.IsNullOrEmpty(localPart) || localPart.StartsWith(".") || localPart.EndsWith("."))
            return false;

        if (!domainPart.Contains("."))
            return false;

        if (domainPart.StartsWith(".") || domainPart.EndsWith("."))
            return false;

        if (domainPart.Contains(".."))
            return false;

        foreach (var c in domainPart)
        {
            if (!char.IsLetterOrDigit(c) && c != '-' && c != '.')
                return false;
        }

        return true;
    }

    public static bool IsEmpty<T>(this IEnumerable<T> collection)
    {
        return !collection.Any();
    }
}