using CarRental.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace CarRental.Application.Services
{
    public class SmtpClientWrapper : ISmtpClient
    {
        private readonly SmtpClient _smtpCLient;

        public SmtpClientWrapper(IConfiguration configuration)
        {
            var smtpServer = configuration["EmailSettings:SmtpServer"];
            var smtpPort = int.Parse(configuration["EmailSettings:SmtpPort"]!);

            _smtpCLient = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(
                    configuration["EmailSettings:SenderEmail"],
                    configuration["EmailSettings:SenderPassword"]
                    ),
                EnableSsl = true
            };
        }
        public async Task SendMailAsync(MailMessage message)
        {
            await _smtpCLient.SendMailAsync(message);
        }
    }  
}
