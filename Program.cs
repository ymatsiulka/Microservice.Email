using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using ArchitectProg.FunctionalExtensions;
using ArchitectProg.Kernel.Extensions;
using ArchitectProg.Kernel.Extensions.Exceptions;
using ArchitectProg.Persistence.EfCore.PostgreSQL;
using ArchitectProg.Persistence.EfCore.PostgreSQL.Settings;
using ArchitectProg.WebApi.Extensions.Filters;
using ArchitectProg.WebApi.Extensions.Responses;
using FluentEmail.Core.Interfaces;
using FluentEmail.Smtp;
using Microservice.Email.Core.Contracts.Requests;
using Microservice.Email.Core.Factories;
using Microservice.Email.Core.Factories.Interfaces;
using Microservice.Email.Core.Mappers;
using Microservice.Email.Core.Mappers.Interfaces;
using Microservice.Email.Core.Services;
using Microservice.Email.Core.Services.Interfaces;
using Microservice.Email.Core.Settings;
using Microservice.Email.Core.Validators;
using Microservice.Email.Core.Validators.Interfaces;
using Microservice.Email.Extensions;
using Microservice.Email.Grpc.Interceptors;
using Microservice.Email.Grpc.Mappers;
using Microservice.Email.Grpc.Mappers.Interfaces;
using Microservice.Email.Grpc.Services;
using Microservice.Email.Infrastructure.Persistence;
using Microservice.Email.Infrastructure.RabbitMQ;
using Microservice.Email.Infrastructure.RabbitMQ.Handlers;
using Microservice.Email.Infrastructure.RabbitMQ.Interfaces;
using Microservice.Email.Interceptors.Metrics;
using Microservice.Email.Smtp;
using Microservice.Email.Smtp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseMetricsWebTracking();
builder.Host.UseMetrics(options =>
{
    options.EndpointOptions = endpointOptions =>
    {
        endpointOptions.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
        endpointOptions.MetricsEndpointOutputFormatter = new MetricsPrometheusProtobufOutputFormatter();
        endpointOptions.EnvironmentInfoEndpointEnabled = false;
    };
});

var configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("grpc", new OpenApiInfo { Title = "Grpc API", Version = "grpc" });
    options.SwaggerDoc("rest", new OpenApiInfo { Title = "REST API", Version = "rest" });

    var filePath = Path.Combine(AppContext.BaseDirectory, "Microservice.Email.xml");
    options.IncludeXmlComments(filePath);
    options.IncludeGrpcXmlComments(filePath, includeControllerXmlComments: true);
});

builder.Services.AddGrpc(x =>
{
    x.Interceptors.Add<ErrorHandlerInterceptor>();
    x.EnableDetailedErrors = true;
}).AddJsonTranscoding();

builder.Services.AddGrpcReflection();
builder.Services.AddScoped<ISendEmailRequestMapper, SendEmailRequestMapper>();
builder.Services.AddScoped<ISendTemplatedEmailRequestMapper, SendTemplatedEmailRequestMapper>();
builder.Services.AddScoped<IEmailResponseMapper, EmailResponseMapper>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new BadRequestOnExceptionFilter(typeof(ValidationException)));
    options.Filters.Add(new BadRequestOnExceptionFilter(typeof(InvalidOperationException)));
    options.Filters.Add(new BadRequestOnExceptionFilter(typeof(ArgumentNullException)));
    options.Filters.Add(new NotFoundOnExceptionFilter(typeof(ResourceNotFoundException)));
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
})
.AddControllersAsServices();

builder.Services.AddKernelExtensions();
builder.Services.AddFunctionalExtensions();
builder.Services.AddEfCoreRepository();
builder.Services.AddMetrics();
builder.Services.AddMetricsTrackingMiddleware();

builder.Services.AddDbContext<DbContext, ApplicationDatabaseContext>();
builder.Services.AddScoped<IHtmlSanitizationService, HtmlSanitizationService>();
builder.Services.AddScoped<ITemplatedEmailService, TemplatedEmailService>();
builder.Services.AddScoped<ISendEmailService, SendEmailService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IRetryPolicyFactory, RetryPolicyFactory>();
builder.Services.AddScoped<IEmailEntityFactory, EmailEntityFactory>();
builder.Services.AddScoped<IAttachmentFactory, AttachmentFactory>();
builder.Services.AddScoped<IAddressFactory, AddressFactory>();
builder.Services.AddScoped<IEmailFactory, EmailFactory>();
builder.Services.AddScoped<IEmailMapper, EmailMapper>();
builder.Services.AddScoped<ISendTemplatedEmailRequestValidator, SendTemplatedEmailRequestValidator>();
builder.Services.AddScoped<IBaseEmailRequestValidator, BaseEmailRequestValidator>();
builder.Services.AddScoped<ISendEmailRequestValidator, SendEmailRequestValidator>();
builder.Services.AddScoped<ISenderValidator, SenderValidator>();
builder.Services.AddScoped<ISmtpClientProvider, SmtpClientProvider>();

builder.Services.AddScoped<ISender>(x =>
{
    var smtpClientProvider = x.GetRequiredService<ISmtpClientProvider>();
    var result = new SmtpSender(smtpClientProvider.SmtpClient);
    return result;
});

builder.Services.Configure<DatabaseSettings>(configuration.GetSection(nameof(DatabaseSettings)));
builder.Services.Configure<RetryPolicySettings>(configuration.GetSection(nameof(RetryPolicySettings)));
builder.Services.Configure<SmtpSettings>(configuration.GetSection(nameof(SmtpSettings)));
builder.Services.Configure<RabbitMQSettings>(configuration.GetSection(nameof(RabbitMQSettings)));

builder.Services.AddRabbitMQBusMessage(messageBus =>
{
    messageBus
        .RegisterExchange("email")
        .RegisterHandler<IRabbitMQMessageHandler<SendEmailRequest>, SendEmailMessageHandler, SendEmailRequest>("sent-email-queue");
});
builder.Services.AddFluentEmail("default_sender@admin.com");

builder.Services.AddInterceptorSingleton<IEmailService, EmailService, CounterMetricInterceptor>();

var app = builder.Build();
app.ApplyMigrations();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/grpc/swagger.json", "Grpc API");
    c.SwaggerEndpoint("/swagger/rest/swagger.json", "REST API");
});

app.UseCors(policy =>
{
    var corsOrigins = configuration
        .GetSection("AllowedCorsOrigins")
        .Get<string[]>();

    if (corsOrigins is not null)
        policy.WithOrigins(corsOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
});

app.UseHttpsRedirection();

app.MapGrpcService<GrpcEmailService>();
app.MapGrpcReflectionService();
app.MapControllers();
app.Run();