namespace Microservice.Email.Extensions;

public static class TypeExtensions
{
    public static string CSharpName(this Type type)
    {
        var typeName = type.Name;

        if (type.IsGenericType)
        {
            var genArgs = type.GetGenericArguments();

            if (genArgs.Length > 0)
            {
                typeName = typeName[..^2];

                var args = string.Empty;

                foreach (var argType in genArgs)
                {
                    var argName = argType.Name;

                    if (argType.IsGenericType)
                        argName = argType.CSharpName();

                    args += argName + ", ";
                }

                typeName = string.Format("{0}<{1}>", typeName, args[..^2]);
            }
        }

        return typeName;
    }
}
