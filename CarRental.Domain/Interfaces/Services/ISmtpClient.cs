using System.Net.Mail;

namespace CarRental.Domain.Interfaces.Services
{
    public interface ISmtpClient
    {
        Task SendMailAsync(MailMessage message);
    }
}
