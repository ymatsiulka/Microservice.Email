using Microservice.Email.Infrastructure.FileStorage.Interfaces;
using Microservice.Email.Infrastructure.FileStorage.Settings;
using Microsoft.Extensions.Options;
using Minio;

namespace Microservice.Email.Infrastructure.FileStorage;

public sealed class FileStorageConnectionProvider : IFileStorageConnectionProvider
{
    private readonly FileStorageSettings settings;
    private readonly Lazy<IMinioClient> client;

    public IMinioClient Client => client.Value;
    public string Bucket => settings.Bucket;

    public FileStorageConnectionProvider(IOptions<FileStorageSettings> settings)
    {
        this.settings = settings.Value;
        client = new Lazy<IMinioClient>(GetClient, LazyThreadSafetyMode.ExecutionAndPublication);
    }

    private IMinioClient GetClient()
    {
        var result = new MinioClient()
            .WithEndpoint(settings.Endpoint)
            .WithCredentials(settings.AccessKey, settings.SecretKey);

        if (settings.EnableSSL)
            result.WithSSL();

        return result.Build();
    }

    public void Dispose()
    {
        if (client.IsValueCreated)
            Client.Dispose();
    }
}