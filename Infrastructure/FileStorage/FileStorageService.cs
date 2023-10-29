using Microservice.Email.Infrastructure.FileStorage.Contracts;
using Microservice.Email.Infrastructure.FileStorage.Interfaces;
using Minio.DataModel.Args;

namespace Microservice.Email.Infrastructure.FileStorage;

public sealed class FileStorageService : IFileStorageService
{
    private readonly IFileStorageConnectionProvider fileStorageConnectionProvider;

    public FileStorageService(IFileStorageConnectionProvider fileStorageConnectionProvider)
    {
        this.fileStorageConnectionProvider = fileStorageConnectionProvider;
    }

    public async Task<UploadResult> Upload(string fileName, string contentType, Stream file)
    {
        var client = fileStorageConnectionProvider.Client;
        var bucket = fileStorageConnectionProvider.Bucket;

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(bucket)
            .WithObject(fileName)
            .WithStreamData(file)
            .WithObjectSize(file.Length)
            .WithContentType(contentType);

        var response = await client.PutObjectAsync(putObjectArgs);
        return new UploadResult
        {
            FileKey = response.ObjectName,
            ContentType = contentType,
            Size = response.Size
        };
    }

    public async Task<InformationResult> Information(string fileName)
    {
        var client = fileStorageConnectionProvider.Client;
        var bucket = fileStorageConnectionProvider.Bucket;

        var statObjectArgs = new StatObjectArgs()
            .WithBucket(bucket)
            .WithObject(fileName);

        var response = await client.StatObjectAsync(statObjectArgs);
        return new InformationResult
        {
            FileKey = response.ObjectName,
            ContentType = response.ContentType,
            LastModified = response.LastModified,
            DeleteMarker = response.DeleteMarker,
            Size = response.Size
        };
    }

    public async Task<DownloadResult> Download(string fileName)
    {
        var client = fileStorageConnectionProvider.Client;
        var bucket = fileStorageConnectionProvider.Bucket;

        var readStream = new MemoryStream();
        var getObjectArgs = new GetObjectArgs()
            .WithBucket(bucket)
            .WithObject(fileName)
            .WithCallbackStream(stream =>
            {
                stream.CopyTo(readStream);
                readStream.Position = 0;
            });

        var response = await client.GetObjectAsync(getObjectArgs);
        return new DownloadResult
        {
            FileKey = response.ObjectName,
            Size = response.Size,
            ContentType = response.ContentType,
            DownloadStream = readStream
        };
    }

    public async Task<RemoveResult> Remove(string fileName)
    {
        var client = fileStorageConnectionProvider.Client;
        var bucket = fileStorageConnectionProvider.Bucket;

        var removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(bucket)
            .WithObject(fileName);

        await client.RemoveObjectAsync(removeObjectArgs);

        return new RemoveResult
        {
            FileKey = fileName
        };
    }
}