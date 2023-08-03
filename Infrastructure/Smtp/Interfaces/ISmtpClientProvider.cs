using System.Net.Mail;

namespace Microservice.Email.Infrastructure.Smtp.Interfaces;

public interface ISmtpClientProvider : IDisposable
{
    public SmtpClient SmtpClient { get; }
}