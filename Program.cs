using ArchitectProg.FunctionalExtensions;
using ArchitectProg.Kernel.Extensions;
using ArchitectProg.Kernel.Extensions.Exceptions;
using ArchitectProg.Persistence.EfCore.PostgreSQL;
using ArchitectProg.Persistence.EfCore.PostgreSQL.Settings;
using ArchitectProg.WebApi.Extensions.Filters;
using ArchitectProg.WebApi.Extensions.Responses;
using Microservice.Email.Creators;
using Microservice.Email.Creators.Interfaces;
using Microservice.Email.Extensions;
using Microservice.Email.Factories;
using Microservice.Email.Factories.Interfaces;
using Microservice.Email.Mappers;
using Microservice.Email.Mappers.Interfaces;
using Microservice.Email.Persistence;
using Microservice.Email.Services;
using Microservice.Email.Services.Interfaces;
using Microservice.Email.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new BadRequestOnExceptionFilter(typeof(ValidationException)));
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
builder.Services.AddDbContext<DbContext, ApplicationDatabaseContext>();

builder.Services.AddScoped<IRetryPolicyFactory, RetryPolicyFactory>();
builder.Services.AddScoped<IAddressFactory, AddressFactory>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IEmailMapper, EmailMapper>();
builder.Services.AddScoped<IEmailCreator, EmailCreator>();
builder.Services.Configure<DatabaseSettings>(configuration.GetSection(nameof(DatabaseSettings)));
builder.Services.Configure<RetryPolicySettings>(configuration.GetSection(nameof(RetryPolicySettings)));
var smtpSettingsConfiguration = configuration.GetSection(nameof(SmtpSettings));
builder.Services.Configure<SmtpSettings>(smtpSettingsConfiguration);
var smtpSettings = smtpSettingsConfiguration.Get<SmtpSettings>() ?? throw new ArgumentNullException(nameof(smtpSettingsConfiguration));
var smtpClient = new SmtpClient(smtpSettings.Host, smtpSettings.Port)
{
    DeliveryMethod = SmtpDeliveryMethod.Network,
    EnableSsl = smtpSettings.EnableSsl,
    Credentials = new NetworkCredential(smtpSettings.Username, smtpSettings.Password)
};


//builder.Services
//    .AddFluentEmail(smtpSettings.Username)
//    .AddRazorRenderer()
//    .AddSmtpSender(smtpClient);

builder.Services
   .AddFluentEmail("admin@local.com")
   .AddRazorRenderer()
   .AddSmtpSender("localhost", 1025);

var app = builder.Build();

app.ApplyMigrations();

app.UseSwagger();
app.UseSwaggerUI();
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

app.MapControllers();
app.Run();