using ArchitectProg.FunctionalExtensions.Extensions;
using Microservice.Email.Core.Services.Interfaces;
using Microservice.Email.Infrastructure.FileStorage.Contracts;
using Microservice.Email.Infrastructure.FileStorage.Interfaces;

namespace Microservice.Email.Core.Services;

public sealed class AttachmentService : IAttachmentService
{
    private readonly IFileStorageService fileStorageService;

    public AttachmentService(IFileStorageService fileStorageService)
    {
        this.fileStorageService = fileStorageService;
    }

    public async Task<UploadResult[]> Upload(IFormFileCollection files)
    {
        var result = await files
            .Select(x => fileStorageService.Upload(x.FileName, x.ContentType, x.OpenReadStream()))
            .WhenAll();

        return result;
    }

    public async Task<InformationResult[]> Information(string[] files)
    {
        var result = await files
            .Select(x => fileStorageService.Information(x))
            .WhenAll();

        return result;
    }

    public async Task<DownloadResult> Download(string fileName)
    {
        var result = await fileStorageService.Download(fileName);
        return result;
    }

    public async Task<RemoveResult[]> Remove(string[] files)
    {
        var result = await files
            .Select(x => fileStorageService.Remove(x))
            .WhenAll();

        return result;
    }
}