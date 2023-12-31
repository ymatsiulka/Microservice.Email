﻿using ArchitectProg.FunctionalExtensions;
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
using Microservice.Email.Modules.Interfaces;

namespace Microservice.Email.Modules;

public sealed class CoreModule : IModule
{
    public void RegisterDependencies(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ITemplateProcessingService, TemplateProcessingService>();
        builder.Services.AddScoped<ISendEmailService, SendEmailService>();
        builder.Services.AddScoped<IEmailService, EmailService>();

        builder.Services.AddScoped<IFormFileAttachmentsValidator, FormFileAttachmentsValidator>();
        builder.Services.AddScoped<IAttachmentsValidator, AttachmentsValidator>();
        builder.Services.AddScoped<ISendTemplatedEmailRequestValidator, SendTemplatedEmailRequestValidator>();
        builder.Services.AddScoped<IBaseEmailRequestValidator, BaseEmailRequestValidator>();
        builder.Services.AddScoped<ISendEmailRequestValidator, SendEmailRequestValidator>();
        builder.Services.AddScoped<ISenderValidator, SenderValidator>();

        builder.Services.AddScoped<IEmailMapper, EmailMapper>();
        builder.Services.AddScoped<IAttachmentMapper, AttachmentMapper>();

        builder.Services.AddScoped<ISendEmailArgsFactory, SendEmailArgsFactory>();
        builder.Services.AddScoped<IRetryPolicyFactory, RetryPolicyFactory>();
        builder.Services.AddScoped<IEmailEntityFactory, EmailEntityFactory>();
        builder.Services.AddScoped<IAddressFactory, AddressFactory>();

        builder.Services.AddKernelExtensions();
        builder.Services.AddFunctionalExtensions();
        
        builder.Services.AddInterceptedScoped<IEmailService, EmailService, CounterMetricInterceptor>();

        var configuration = builder.Configuration;
        builder.Services.Configure<RetryPolicySettings>(configuration.GetSection(nameof(RetryPolicySettings)));
    }
}
