using ArchitectProg.Kernel.Extensions.Entities;
using Microservice.Email.Domain.Enums;
using Microsoft.Extensions.Hosting;

namespace Microservice.Email.Domain.Entities;

public sealed class EmailEntity : Entity<int>
{
    public required string Body { get; init; }
    public required string Subject { get; init; }
    public required string SenderName { get; init; }
    public required string SenderEmail { get; init; }
    public required DateTimeOffset SentDate { get; init; }
    public required EmailStatus EmailStatus { get; init; }

    public ICollection<RecipientEntity> Recipients { get; set; } = [];
}