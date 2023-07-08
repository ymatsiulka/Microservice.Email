using ArchitectProg.Persistence.EfCore.PostgreSQL.Settings;
using Microservice.Email.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Microservice.Email.Persistence;

public sealed class ApplicationDatabaseContext : DbContext
{
    private readonly DatabaseSettings databaseSettings;

    public DbSet<EmailEntity> Areas => Set<EmailEntity>();

    public ApplicationDatabaseContext(IOptions<DatabaseSettings> databaseSettings)
    {
        this.databaseSettings = databaseSettings.Value;

        ChangeTracker.LazyLoadingEnabled = false;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql(databaseSettings.ConnectionString)
            .UseSnakeCaseNamingConvention();
    }
}
