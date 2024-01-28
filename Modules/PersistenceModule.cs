using ArchitectProg.Persistence.EfCore.PostgreSQL;
using ArchitectProg.Persistence.EfCore.PostgreSQL.Settings;
using Microservice.Email.Infrastructure.Persistence;
using Microservice.Email.Modules.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Email.Modules;

public sealed class PersistenceModule : IModule
{
    public void RegisterDependencies(WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        var databaseSettingsConfigurationSection = configuration.GetSection(nameof(DatabaseSettings));

        builder.Services.AddDatabase<ApplicationDatabaseContext>(databaseSettingsConfigurationSection); 
        builder.Services.AddDbContext<DbContext, ApplicationDatabaseContext>();
    }
}
