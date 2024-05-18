using Microservice.Email.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace Microservice.Email.Infrastructure.Persistence.Configuration;

public sealed class RecipientEntityTypeConfiguration : IEntityTypeConfiguration<RecipientEntity>
{
    public void Configure(EntityTypeBuilder<RecipientEntity> builder)
    {
        builder.Property(t => t.Email);

        builder.ToTable("TRecipient");


    }
}