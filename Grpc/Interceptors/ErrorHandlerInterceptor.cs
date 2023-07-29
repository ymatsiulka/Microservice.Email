using ArchitectProg.Kernel.Extensions.Exceptions;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microservice.Email.Extensions;

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
        var message = exception.Message.StripUnicodeCharacters();
        var stackTrace = exception.StackTrace.StripUnicodeCharacters();
        var exceptionType = exception.GetType().Name;

        var metadata = new Metadata
        {
            { nameof(exceptionType), exceptionType },
            { nameof(exception.Message), message },
            { nameof(exception.StackTrace), stackTrace }
        };

        var result = new RpcException(new Status(statusCode, statusCode.ToString()), metadata);
        return result;
    }
}