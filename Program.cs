using Microservice.Email.Extensions;
using Microservice.Email.Grpc.Services;
using Microservice.Email.Modules;
using Microservice.Email.Modules.Interfaces;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

var modules = new IModule[]
{
    new ApiModule(),
    new PersistenceModule(),
    new CoreModule(),
    new BusModule(),
    new GrpcModule(),
    new SmtpModule(),
};

foreach (var module in modules)
    module.RegisterDependencies(builder);

var configuration = builder.Configuration;

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

app.MapControllers();
app.UseHttpsRedirection();
app.MapGrpcService<GrpcEmailService>();
app.MapGrpcReflectionService();

app.UseMetricServer();
app.UseHttpMetrics();
app.Run();