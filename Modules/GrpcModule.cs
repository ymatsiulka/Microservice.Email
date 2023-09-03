using Microservice.Email.Grpc.Interceptors;
using Microservice.Email.Grpc.Mappers;
using Microservice.Email.Grpc.Mappers.Interfaces;
using Microservice.Email.Modules.Interfaces;

namespace Microservice.Email.Modules;

public sealed class GrpcModule : IModule
{
    public void RegisterDependencies(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ISendTemplatedEmailRequestMapper, SendTemplatedEmailRequestMapper>();
        builder.Services.AddScoped<ISendEmailRequestMapper, SendEmailRequestMapper>();
        builder.Services.AddScoped<IEmailResponseMapper, EmailResponseMapper>();
        builder.Services.AddScoped<IAttachmentMapper, AttachmentMapper>();

        builder.Services.AddGrpc(x =>
        {
            x.Interceptors.Add<ErrorHandlerInterceptor>();
            x.EnableDetailedErrors = true;
        }).AddJsonTranscoding();

        builder.Services.AddGrpcReflection();
        builder.Services.AddGrpcSwagger();
    }
}
