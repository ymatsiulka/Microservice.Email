using Microservice.Email.Domain.Entities;
using Microservice.Email.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservice.Email.Persistence.Configuration;

public sealed class EmailEntityTypeConfiguration : IEntityTypeConfiguration<EmailEntity>
{



    public void Configure(EntityTypeBuilder<EmailEntity> builder)
    {
        builder.ToTable("Email");

        builder.Property(p => p.Recipients)
            .HasMaxLength(8000)
            .SetPropertyConverterAndComparer();
    }


}