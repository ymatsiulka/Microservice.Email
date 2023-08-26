using Microservice.Email.Grpc.Interceptors;
using Microservice.Email.Modules.Interfaces;

namespace Microservice.Email.Modules;

public sealed class GrpcModule : IModule
{
    public void RegisterDependencies(WebApplicationBuilder builder)
    {
        builder.Services.AddGrpc(x =>
        {
            x.Interceptors.Add<ErrorHandlerInterceptor>();
            x.EnableDetailedErrors = true;
        }).AddJsonTranscoding();

        builder.Services.AddGrpcReflection();
        builder.Services.AddGrpcSwagger();
    }
}
