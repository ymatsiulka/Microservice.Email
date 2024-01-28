using ArchitectProg.FunctionalExtensions.Extensions;
using ArchitectProg.Kernel.Extensions.Exceptions;
using Microservice.Email.Core.Attributes;
using Microservice.Email.Core.Contracts.Requests;
using Microservice.Email.Core.Contracts.Responses;
using Microservice.Email.Core.Factories.Interfaces;
using Microservice.Email.Core.Mappers.Interfaces;
using Microservice.Email.Core.Services.Interfaces;
using Microservice.Email.Core.Validators.Interfaces;
using Microservice.Email.Infrastructure.FileStorage.Interfaces;

namespace Microservice.Email.Core.Services;

public sealed class EmailService : IEmailService
{
    private readonly ISendEmailService sendEmailService;
    private readonly ISendEmailArgsFactory emailArgsFactory;
    private readonly IFileStorageService fileStorageService;
    private readonly ITemplateProcessingService templateProcessingService;
    private readonly IAttachmentMapper attachmentMapper;
    private readonly ISendEmailRequestValidator sendEmailRequestValidator;
    private readonly ISendTemplatedEmailRequestValidator sendTemplatedEmailRequestValidator;

    public EmailService(
        ISendEmailService sendEmailService,
        ISendEmailArgsFactory emailArgsFactory,
        IFileStorageService fileStorageService,
        ITemplateProcessingService templateProcessingService,
        IAttachmentMapper attachmentMapper,
        ISendEmailRequestValidator sendEmailRequestValidator,
        ISendTemplatedEmailRequestValidator sendTemplatedEmailRequestValidator)
    {
        this.sendEmailService = sendEmailService;
        this.emailArgsFactory = emailArgsFactory;
        this.fileStorageService = fileStorageService;
        this.templateProcessingService = templateProcessingService;
        this.attachmentMapper = attachmentMapper;
        this.sendEmailRequestValidator = sendEmailRequestValidator;
        this.sendTemplatedEmailRequestValidator = sendTemplatedEmailRequestValidator;
    }

    [CounterMetric("send_email", "Number of sent emails")]
    public async Task<EmailResponse> Send(AttachmentsWrapper<SendEmailRequest> request)
    {
        var errors = sendEmailRequestValidator.Validate(request).ToArray();
        if (errors.Any())
            throw new ValidationException(errors);

        var attachments = request.Attachments ?? Array.Empty<Attachment>();
        var loadedAttachments = await attachments.Select(x => fileStorageService.Download(x.FileName)).WhenAll();
        var attachmentsArgs = attachmentMapper.MapCollection(loadedAttachments).ToArray();

        var args = emailArgsFactory.Create(request.Email, attachmentsArgs);
        var result = await sendEmailService.Send(args);
        return result;
    }

    [CounterMetric("send_templated_email", "Number of sent templated emails")]
    public async Task<EmailResponse> SendTemplated(AttachmentsWrapper<SendTemplatedEmailRequest> request)
    {
        var errors = sendTemplatedEmailRequestValidator.Validate(request).ToArray();
        if (errors.Any())
            throw new ValidationException(errors);

        var content = await templateProcessingService.Process(request.Email);

        var attachments = request.Attachments ?? Array.Empty<Attachment>();
        var loadedAttachments = await attachments.Select(x => fileStorageService.Download(x.FileName)).WhenAll();
        var attachmentsArgs = attachmentMapper.MapCollection(loadedAttachments).ToArray();

        var args = emailArgsFactory.Create(request.Email, content, attachmentsArgs);
        var result = await sendEmailService.Send(args);
        return result;
    }

    [CounterMetric("send_email_with_form_files", "Number of sent emails with form files")]
    public async Task<EmailResponse> SendWithFormFiles(FormFilesWrapper<SendEmailRequest> request)
    {
        var errors = sendEmailRequestValidator.Validate(request).ToArray();
        if (errors.Any())
            throw new ValidationException(errors);

        var attachments = request.Attachments?.ToArray() ?? Array.Empty<IFormFile>();
        var attachmentsArgs = attachmentMapper.MapCollection(attachments).ToArray();

        var args = emailArgsFactory.Create(request.Email, attachmentsArgs);
        var result = await sendEmailService.Send(args);
        return result;
    }

    [CounterMetric("send_templated_email_with_form_files", "Number of sent templated emails with form files")]
    public async Task<EmailResponse> SendTemplatedWithFormFiles(
        FormFilesWrapper<SendTemplatedEmailRequest> request)
    {
        var errors = sendTemplatedEmailRequestValidator.Validate(request).ToArray();
        if (errors.Any())
            throw new ValidationException(errors);

        var content = await templateProcessingService.Process(request.Email);
        var attachments = request.Attachments?.ToArray() ?? Array.Empty<IFormFile>();
        var attachmentsArgs = attachmentMapper.MapCollection(attachments).ToArray();

        var args = emailArgsFactory.Create(request.Email, content, attachmentsArgs);
        var result = await sendEmailService.Send(args);
        return result;
    }
}