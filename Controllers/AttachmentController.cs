using ArchitectProg.WebApi.Extensions.Attributes;
using Microservice.Email.Core.Services.Interfaces;
using Microservice.Email.Infrastructure.FileStorage.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Email.Controllers;

[ApiController]
[ApiExplorerSettings(GroupName = "rest")]
[Route("api/attachment")]
public sealed class AttachmentController : ControllerBase
{
    private readonly IAttachmentService attachmentService;

    public AttachmentController(IAttachmentService attachmentService)
    {
        this.attachmentService = attachmentService;
    }

    [ProducesBadRequest]
    [ProducesOk(typeof(UploadResult[]))]
    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] IFormFileCollection files)
    {
        var result = await attachmentService.Upload(files);
        return Ok(result);
    }

    [ProducesBadRequest]
    [ProducesOk(typeof(DownloadResult))]
    [HttpGet("info")]
    public async Task<IActionResult> Info([FromQuery] string[] files)
    {
        var result = await attachmentService.Information(files);
        return Ok(result);
    }

    [ProducesBadRequest]
    [ProducesOk(typeof(DownloadResult))]
    [HttpGet("download")]
    public async Task<IActionResult> Download([FromQuery] string fileName)
    {
        var result = await attachmentService.Download(fileName);
        return File(result.DownloadStream, result.ContentType, result.FileKey);

    }

    [ProducesBadRequest]
    [ProducesOk(typeof(RemoveResult))]
    [HttpDelete("remove")]
    public async Task<IActionResult> Remove([FromQuery] string[] files)
    {
        var result = await attachmentService.Remove(files);
        return Ok(result);

    }
}