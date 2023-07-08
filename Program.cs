using ArchitectProg.FunctionalExtensions;
using ArchitectProg.Kernel.Extensions;
using ArchitectProg.Kernel.Extensions.Exceptions;
using ArchitectProg.Persistence.EfCore.PostgreSQL;
using ArchitectProg.Persistence.EfCore.PostgreSQL.Settings;
using ArchitectProg.WebApi.Extensions.Filters;
using ArchitectProg.WebApi.Extensions.Responses;
using Microservice.Email.Persistence;
using Microservice.Email.Persistence.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
builder.Services.Configure<DatabaseSettings>(configuration.GetSection(nameof(DatabaseSettings)));

var app = builder.Build();

app.ApplyMigrations();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(policy =>
{
    var corsOrigins = configuration
        .GetSection("AllowedCorsOrigins")
        .Get<string[]>();

    policy.WithOrigins(corsOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod();
});

app.UseHttpsRedirection();

app.MapControllers();
app.Run();