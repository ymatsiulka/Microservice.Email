using ArchitectProg.FunctionalExtensions;
using ArchitectProg.Kernel.Extensions;
using Microservice.Email.Core.Factories;
using Microservice.Email.Core.Factories.Interfaces;
using Microservice.Email.Core.Interceptors.Metrics;
using Microservice.Email.Core.Mappers;
using Microservice.Email.Core.Mappers.Interfaces;
using Microservice.Email.Core.Services;
using Microservice.Email.Core.Services.Interfaces;
using Microservice.Email.Core.Settings;
using Microservice.Email.Core.Validators;
using Microservice.Email.Core.Validators.Interfaces;
using Microservice.Email.Extensions;
using Microservice.Email.Grpc.Mappers;
using Microservice.Email.Grpc.Mappers.Interfaces;
using Microservice.Email.Modules.Interfaces;

namespace Microservice.Email.Modules;

public sealed class CoreModule : IModule
{
    public void RegisterDependencies(WebApplicationBuilder builder)
    {
        builder.Services.AddInterceptedScoped<IEmailService, EmailService, CounterMetricInterceptor>();

        builder.Services.AddScoped<IHtmlSanitizationService, HtmlSanitizationService>();
        builder.Services.AddScoped<ITemplatedEmailService, TemplatedEmailService>();
        builder.Services.AddScoped<ISendEmailService, SendEmailService>();
        builder.Services.AddScoped<IEmailService, EmailService>();

        builder.Services.AddScoped<ISendTemplatedEmailRequestValidator, SendTemplatedEmailRequestValidator>();
        builder.Services.AddScoped<IBaseEmailRequestValidator, BaseEmailRequestValidator>();
        builder.Services.AddScoped<ISendEmailRequestValidator, SendEmailRequestValidator>();
        builder.Services.AddScoped<ISenderValidator, SenderValidator>();

        builder.Services.AddScoped<ISendTemplatedEmailRequestMapper, SendTemplatedEmailRequestMapper>();
        builder.Services.AddScoped<ISendEmailRequestMapper, SendEmailRequestMapper>();
        builder.Services.AddScoped<IEmailResponseMapper, EmailResponseMapper>();
        builder.Services.AddScoped<IEmailMapper, EmailMapper>();

        builder.Services.AddScoped<IRetryPolicyFactory, RetryPolicyFactory>();
        builder.Services.AddScoped<IEmailEntityFactory, EmailEntityFactory>();
        builder.Services.AddScoped<IAttachmentFactory, AttachmentFactory>();
        builder.Services.AddScoped<IAddressFactory, AddressFactory>();
        builder.Services.AddScoped<IEmailFactory, EmailFactory>();

        builder.Services.AddKernelExtensions();
        builder.Services.AddFunctionalExtensions();

        var configuration = builder.Configuration;
        builder.Services.Configure<RetryPolicySettings>(configuration.GetSection(nameof(RetryPolicySettings)));
    }
}
