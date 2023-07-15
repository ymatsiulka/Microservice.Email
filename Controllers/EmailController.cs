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

    public EmailController(IEmailService emailService)
    {
        this.emailService = emailService;
    }

    [ProducesBadRequest]
    [ProducesOk(typeof(EmailResponse))]
    [HttpPost("send")]
    public async Task<IActionResult> Send([FromForm]SendEmailRequest request)
    {
        var result = await emailService.Send(request);
        var response = result.MatchActionResult(Ok);
        return response;
    }

    [ProducesBadRequest]
    [ProducesOk(typeof(EmailResponse))]
    [HttpPost("templated/send")]
    public async Task<IActionResult> SendTemplatedEmail(SendTemplatedEmailRequest request)
    {
        var result = await emailService.SendTemplated(request);
        var response = result.MatchActionResult(Ok);
        return response;
    }
}