using System.Net;
using ArchitectProg.Kernel.Extensions.Exceptions;
using Grpc.Core;

namespace Microservice.Email.Grpc.Utils;

public static class ExceptionHandler
{
    public static async Task<T> TryExecuteOperation<T>(Func<Task<T>> operation)
    {
        try
        {
            return await operation();
        }
        catch (ResourceNotFoundException ex)
        {
            throw GetRpcException(StatusCode.NotFound, ex);
        }
        catch (Exception ex) when (ex is ValidationException or InvalidOperationException or ArgumentNullException)
        {
            throw GetRpcException(StatusCode.InvalidArgument, ex);
        }
        catch (Exception ex)
        {
            throw GetRpcException(StatusCode.Unknown, ex);
        }
    }

    public static RpcException GetRpcException(StatusCode statusCode, Exception exception)
    {
        var message = WebUtility.UrlEncode(exception.Message);
        var stackTrace = WebUtility.UrlEncode(exception.StackTrace ?? string.Empty);
        var metadata = new Metadata
        {
            { "message", message },
            { "stackTrace", stackTrace }
        };

        var result = new RpcException(new Status(statusCode, statusCode.ToString()), metadata);
        return result;
    }
}