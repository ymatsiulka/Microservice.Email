﻿using Grpc.Contracts.Email;
using Grpc.Core;
using Grpc.Services;
using Microservice.Email.Core.Services.Interfaces;
using Microservice.Email.Extensions;
using Microservice.Email.Grpc.Mappers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Email.Grpc.Services;

[ApiExplorerSettings(GroupName = "grpc")]
public sealed class GrpcEmailService : EmailService.EmailServiceBase
{
    private readonly IEmailService emailService;
    private readonly IEmailResponseMapper emailResponseMapper;
    private readonly ISendEmailRequestMapper sendEmailRequestMapper;
    private readonly ISendTemplatedEmailRequestMapper sendTemplatedEmailRequestMapper;

    public GrpcEmailService(
        IEmailService emailService,
        IEmailResponseMapper emailResponseMapper,
        ISendEmailRequestMapper sendEmailRequestMapper,
        ISendTemplatedEmailRequestMapper sendTemplatedEmailRequestMapper)
    {
        this.emailService = emailService;
        this.emailResponseMapper = emailResponseMapper;
        this.sendEmailRequestMapper = sendEmailRequestMapper;
        this.sendTemplatedEmailRequestMapper = sendTemplatedEmailRequestMapper;
    }

    public override async Task<GrpcEmailResponse> Send(
        GrpcSendEmailRequest request,
        ServerCallContext context)
    {
        var processedRequest = sendEmailRequestMapper.Map(request);
        var result = await emailService.Send(processedRequest);
        var response = emailResponseMapper.Map(result);
        return response;
    }

    public override async Task<GrpcEmailResponse> SendTemplated(
        GrpcSendTemplatedEmailRequest request,
        ServerCallContext context)
    {
        var processedRequest = sendTemplatedEmailRequestMapper.Map(request);
        var result = await emailService.SendTemplated(processedRequest);
        var response = emailResponseMapper.Map(result);
        return response;
    }
}