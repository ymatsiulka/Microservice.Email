namespace Microservice.Email.Core.Extensions;

public static class ValidationExtensions
{
    public static bool IsValidEmail(this string email)
    {
        var index = email.IndexOf('@');

        return
            index > 0 &&
            index != email.Length - 1 &&
            index == email.LastIndexOf('@');
    }

    public static bool IsEmpty<T>(this IEnumerable<T> collection)
    {
        return !collection.Any();
    }
}