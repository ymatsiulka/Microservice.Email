using System.Net;
using ArchitectProg.Kernel.Extensions.Exceptions;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Microservice.Email.Grpc.Interceptors;

public class ErrorHandlerInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            var response = await base.UnaryServerHandler(request, context, continuation);
            return response;
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

    private static RpcException GetRpcException(StatusCode statusCode, Exception exception)
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