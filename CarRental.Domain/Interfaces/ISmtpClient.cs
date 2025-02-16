using System.Net.Mail;

namespace CarRental.Domain.Interfaces
{
    public interface ISmtpClient
    {
        Task SendMailAsync(MailMessage message);
    }
}
