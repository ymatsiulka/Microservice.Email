using Microservice.Email.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservice.Email.Infrastructure.Persistence.Configuration;

public sealed class EmailEntityTypeConfiguration : IEntityTypeConfiguration<EmailEntity>
{
    public void Configure(EntityTypeBuilder<EmailEntity> builder)
    {
        builder
          .HasMany(e => e.Recipients)
          .WithOne(e => e.EmailEntity)
          .HasForeignKey(e => e.EmailId)
          .IsRequired();


        builder.ToTable("TEmail");
    }
}