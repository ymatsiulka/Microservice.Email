using System.Net.Mail;

namespace Microservice.Email.Smtp.Interfaces;

public interface ISmtpClientProvider : IDisposable
{
    public SmtpClient SmtpClient { get; }
}