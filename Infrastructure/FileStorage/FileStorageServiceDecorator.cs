using Microservice.Email.Infrastructure.FileStorage.Contracts;
using Microservice.Email.Infrastructure.FileStorage.Interfaces;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace Microservice.Email.Infrastructure.FileStorage;

public sealed class FileStorageServiceDecorator : IFileStorageService
{
    private readonly IFileStorageService fileStorageService;
    private readonly ILogger<FileStorageServiceDecorator> logger;
    private readonly IFileStorageConnectionProvider fileStorageConnectionProvider;

    public FileStorageServiceDecorator(
        IFileStorageService fileStorageService,
        ILogger<FileStorageServiceDecorator> logger,
        IFileStorageConnectionProvider fileStorageConnectionProvider)
    {
        this.logger = logger;
        this.fileStorageService = fileStorageService;
        this.fileStorageConnectionProvider = fileStorageConnectionProvider;
    }

    public async Task<UploadResult> Upload(string fileName, string contentType, Stream file)
    {
        try
        {
            await CreateBucketIfNotExists();
            var result = await fileStorageService.Upload(fileName, contentType, file);
            return result;
        }
        catch (MinioException e)
        {
            logger.LogError("Failed to upload file. Error: {Message}", e.Message);
            throw;
        }
    }

    public async Task<InformationResult> Information(string fileName)
    {
        try
        {
            await CreateBucketIfNotExists();
            var result = await fileStorageService.Information(fileName);
            return result;
        }
        catch (MinioException e)
        {
            logger.LogError("Failed to retrieve file information. Error: {Message}", e.Message);
            throw;
        }
    }

    public async Task<DownloadResult> Download(string fileName)
    {
        try
        {
            await CreateBucketIfNotExists();
            var result = await fileStorageService.Download(fileName);
            return result;
        }
        catch (MinioException e)
        {
            logger.LogError("Failed to download file. Error: {Message}", e.Message);
            throw;
        }
    }

    public async Task<RemoveResult> Remove(string fileName)
    {
        try
        {
            await CreateBucketIfNotExists();
            var result = await fileStorageService.Remove(fileName);
            return result;
        }
        catch (MinioException e)
        {
            logger.LogError("Failed to remove file. Error: {Message}", e.Message);
            throw;
        }
    }

    private async Task CreateBucketIfNotExists()
    {
        var client = fileStorageConnectionProvider.Client;
        var bucket = fileStorageConnectionProvider.Bucket;

        var existsArgs = new BucketExistsArgs().WithBucket(bucket);

        if (await client.BucketExistsAsync(existsArgs))
            return;

        var makeArgs = new MakeBucketArgs().WithBucket(bucket);
        await client.MakeBucketAsync(makeArgs);
    }
}