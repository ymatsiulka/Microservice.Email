using ArchitectProg.WebApi.Extensions.Attributes;
using Microservice.Email.Contracts.Requests;
using Microservice.Email.Contracts.Responses;
using Microservice.Email.Core.Services.Interfaces;
using Microservice.Email.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Email.Controllers;

[ApiController]
[Route("api/email")]
public sealed class EmailController : ControllerBase
{
    private readonly IEmailService emailService;
    private readonly ITemplatedEmailService templatedEmailService;

    public EmailController(
        IEmailService emailService,
        ITemplatedEmailService templatedEmailService)
    {
        this.emailService = emailService;
        this.templatedEmailService = templatedEmailService;
    }

    [ProducesBadRequest]
    [ProducesOk(typeof(EmailSendResponse))]
    [HttpPost("send")]
    public async Task<IActionResult> Send([FromForm] SendEmailRequest request)
    {
        var result = await emailService.Send(request);
        var response = result.MatchActionResult(Ok);
        return response;
    }

    [ProducesBadRequest]
    [ProducesOk(typeof(EmailSendResponse))]
    [HttpPost("templated/send")]
    public async Task<IActionResult> SendTemplatedEmail([FromForm] SendTemplatedEmailRequest request)
    {
        var sendRequest = await templatedEmailService.ProcessTemplatedRequest(request);
        var result = await emailService.Send(sendRequest);
        var response = result.MatchActionResult(Ok);
        return response;
    }
}