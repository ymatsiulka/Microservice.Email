using Microservice.Email.Core.Services;
using Microservice.Email.Core.Services.Interfaces;
using Microservice.Email.Infrastructure.FileStorage;
using Microservice.Email.Infrastructure.FileStorage.Interfaces;
using Microservice.Email.Infrastructure.FileStorage.Settings;
using Microservice.Email.Modules.Interfaces;

namespace Microservice.Email.Modules;

public sealed class FileStorageModule : IModule
{
    public void RegisterDependencies(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IFileStorageConnectionProvider, FileStorageConnectionProvider>();
        builder.Services.AddScoped<IFileStorageService, FileStorageService>();
        builder.Services.Decorate<IFileStorageService, FileStorageServiceDecorator>();

        builder.Services.AddScoped<IAttachmentService, AttachmentService>();

        var configuration = builder.Configuration;
        builder.Services.Configure<FileStorageSettings>(configuration.GetSection(nameof(FileStorageSettings)));
    }
}
