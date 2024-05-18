using ArchitectProg.Kernel.Extensions.Entities;

namespace Microservice.Email.Domain.Entities;

public class RecipientEntity : Entity<int>
{
    public required string Email { get; set; }
    public int EmailId { get; set; }
    public EmailEntity EmailEntity { get; set; } = null!;
}

