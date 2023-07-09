using Microservice.Email.Domain.Entities;
using Microservice.Email.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

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