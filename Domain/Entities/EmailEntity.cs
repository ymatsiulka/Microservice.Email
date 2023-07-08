using ArchitectProg.Kernel.Extensions.Entities;
using Microservice.Email.Domain.Enums;

namespace Microservice.Email.Domain.Entities;

public class EmailEntity : Entity<int>
{
    public string? Body { get; init; }
    public required string Subject { get; init; }
    public required string Sender { get; init; }
    public required string[] Recipients { get; init; }
    public required DateTimeOffset SentDate { get; init; }
    public required EmailStatus EmailStatus { get; init; }
}