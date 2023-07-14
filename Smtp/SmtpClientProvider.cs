using System.Net;
using System.Net.Mail;
using ArchitectProg.FunctionalExtensions.Extensions;
using Microservice.Email.Smtp.Interfaces;
using Microsoft.Extensions.Options;

namespace Microservice.Email.Smtp
{
    public sealed class SmtpClientProvider : ISmtpClientProvider
    {
        private readonly SmtpSettings smtpSettings;
        private readonly Lazy<SmtpClient> smtpClient;

        public SmtpClient SmtpClient => smtpClient.Value;

        public SmtpClientProvider(IOptions<SmtpSettings> smtpSettings)
        {
            this.smtpSettings = smtpSettings.Value;
            smtpClient = new Lazy<SmtpClient>(GetSmtpClient);
        }

        private SmtpClient GetSmtpClient()
        {
            var hasCredentials = !smtpSettings.Username.IsNullOrWhiteSpace() && !smtpSettings.Password.IsNullOrWhiteSpace();
            var credentials = hasCredentials
                ? new NetworkCredential(smtpSettings.Username, smtpSettings.Password)
                : null;

            var result = new SmtpClient
            {
                Host = smtpSettings.Host,
                Port = smtpSettings.Port,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = smtpSettings.EnableSsl,
                UseDefaultCredentials = smtpSettings.UseDefaultCredentials,
                Credentials = credentials
            };

            return result;
        }

        public void Dispose()
        {
            if(smtpClient.IsValueCreated)
                SmtpClient.Dispose();
        }
    }
}