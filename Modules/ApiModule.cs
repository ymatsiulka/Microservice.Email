using ArchitectProg.Kernel.Extensions.Exceptions;
using ArchitectProg.WebApi.Extensions.Filters;
using ArchitectProg.WebApi.Extensions.Responses;
using Microservice.Email.Modules.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Minio.Exceptions;

namespace Microservice.Email.Modules;

public sealed class ApiModule : IModule
{
    public void RegisterDependencies(WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllers(options =>
        {
            options.Filters.Add(new BadRequestOnExceptionFilter(typeof(ValidationException)));
            options.Filters.Add(new BadRequestOnExceptionFilter(typeof(InvalidOperationException)));
            options.Filters.Add(new BadRequestOnExceptionFilter(typeof(ArgumentNullException)));
            options.Filters.Add(new NotFoundOnExceptionFilter(typeof(ResourceNotFoundException)));
            options.Filters.Add(new NotFoundOnExceptionFilter(typeof(ObjectNotFoundException)));
        })
        .ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var messages = context.ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage);

                var message = string.Join(" ", messages);

                var response = new ErrorResult(StatusCodes.Status400BadRequest, message);
                return new BadRequestObjectResult(response);
            };
        }).AddControllersAsServices();

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
