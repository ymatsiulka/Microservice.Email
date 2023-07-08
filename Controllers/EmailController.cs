using ArchitectProg.WebApi.Extensions.Attributes;
using Microservice.Email.Contracts.Requests;
using Microservice.Email.Contracts.Responses;
using Microservice.Email.Extensions;
using Microservice.Email.Services.Interfaces;
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
    public async Task<IActionResult> Send(SendEmailRequest request)
    {
        var result = await emailService.Send(request);
        var response = result.MatchActionResult(x => Ok(x));
        return response;
    }
}