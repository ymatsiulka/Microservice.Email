using FluentEmail.Core.Interfaces;
using FluentEmail.Smtp;
using Microservice.Email.Infrastructure.Smtp;
using Microservice.Email.Infrastructure.Smtp.Interfaces;
using Microservice.Email.Infrastructure.Smtp.Settings;
using Microservice.Email.Modules.Interfaces;

namespace Microservice.Email.Modules;

public sealed class SmtpModule : IModule
{
    public void RegisterDependencies(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ISmtpClientProvider, SmtpClientProvider>();
        builder.Services.AddScoped<ISender>(x =>
        {
            var smtpClientProvider = x.GetRequiredService<ISmtpClientProvider>();
            var result = new SmtpSender(smtpClientProvider.SmtpClient);
            return result;
        });
        builder.Services.AddFluentEmail("default_sender@admin.com");

        var configuration = builder.Configuration;
        builder.Services.Configure<SmtpSettings>(configuration.GetSection(nameof(SmtpSettings)));
    }
}
