using Microservice.Email.Infrastructure.Persistence;
using Microservice.Email.Modules.Interfaces;
using Microsoft.EntityFrameworkCore;
using Yurutaru.Platform.NetCore.Persistence.EfCore.PostgreSQL;
using Yurutaru.Platform.NetCore.Persistence.EfCore.PostgreSQL.Settings;

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
