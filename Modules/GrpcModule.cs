using Microservice.Email.Grpc.Interceptors;
using Microservice.Email.Modules.Interfaces;
using Microsoft.OpenApi.Models;

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
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("grpc", new OpenApiInfo { Title = "Grpc API", Version = "grpc" });
            options.SwaggerDoc("rest", new OpenApiInfo { Title = "REST API", Version = "rest" });

            var filePath = Path.Combine(AppContext.BaseDirectory, "Microservice.Email.xml");
            options.IncludeXmlComments(filePath);
            options.IncludeGrpcXmlComments(filePath, includeControllerXmlComments: true);
        });
    }
}
