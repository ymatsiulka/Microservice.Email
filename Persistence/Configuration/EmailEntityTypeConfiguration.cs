using Microservice.Email.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservice.Email.Persistence.Configuration
{
    public sealed class EmailEntityTypeConfiguration : IEntityTypeConfiguration<EmailEntity>
    {
        public void Configure(EntityTypeBuilder<EmailEntity> builder)
        {
            builder.ToTable("Email");

            builder
                .Property(t => t.Recipients)
                .HasColumnType("jsonb");
        }
    }
}
