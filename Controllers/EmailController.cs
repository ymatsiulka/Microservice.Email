using ArchitectProg.WebApi.Extensions.Attributes;
using Microservice.Email.Core.Contracts.Requests;
using Microservice.Email.Core.Contracts.Responses;
using Microservice.Email.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Email.Controllers;

[ApiController]
[ApiExplorerSettings(GroupName = "rest")]
[Route("api/email")]
public sealed class EmailController : ControllerBase
{
    private readonly IEmailService emailService;

    public EmailController(IEmailService emailService)
    {
        this.emailService = emailService;
    }

    [ProducesBadRequest]
    [ProducesOk(typeof(EmailResponse))]
    [HttpPost("send")]
    public async Task<IActionResult> Send(AttachmentsWrapper<SendEmailRequest> request)
    {
        var result = await emailService.Send(request);
        return Ok(result);
    }

    [ProducesBadRequest]
    [ProducesOk(typeof(EmailResponse))]
    [HttpPost("send/templated")]
    public async Task<IActionResult> SendTemplatedEmail(AttachmentsWrapper<SendTemplatedEmailRequest> request)
    {
        var result = await emailService.SendTemplated(request);
        return Ok(result);

    }

    [ProducesBadRequest]
    [ProducesOk(typeof(EmailResponse))]
    [HttpPost("send/formFiles")]
    public async Task<IActionResult> Send([FromForm] FormFilesWrapper<SendEmailRequest> request)
    {
        var result = await emailService.SendWithFormFiles(request);
        return Ok(result);
    }

    [ProducesBadRequest]
    [ProducesOk(typeof(EmailResponse))]
    [HttpPost("send/templated/formFiles")]
    public async Task<IActionResult> SendTemplatedEmail([FromForm] FormFilesWrapper<SendTemplatedEmailRequest> request)
    {
        var result = await emailService.SendTemplatedWithFormFiles(request);
        return Ok(result);
    }
}