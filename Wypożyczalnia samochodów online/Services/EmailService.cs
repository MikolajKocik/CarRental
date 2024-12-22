using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Wypożyczalnia_samochodów_online.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var smtpServer = _configuration["EmailSettings:SmtpServer"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
                var senderEmail = _configuration["EmailSettings:SenderEmail"];
                var senderName = _configuration["EmailSettings:SenderName"];
                var senderPassword = _configuration["EmailSettings:SenderPassword"];

                using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                    smtpClient.EnableSsl = true; // SSL jest włączone

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(senderEmail, senderName),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };
                    mailMessage.To.Add(to);

                    _logger.LogInformation($"Wysyłanie e-maila do {to} z tematem '{subject}'.");

                    await smtpClient.SendMailAsync(mailMessage);

                    _logger.LogInformation($"E-mail do {to} został pomyślnie wysłany.");
                }
            }
            catch (SmtpException smtpEx)
            {
                _logger.LogError($"Błąd SMTP podczas wysyłania e-maila do {to}: {smtpEx.Message}");
                throw; // Przekazuje wyjątek dalej, aby można było go obsłużyć wyżej
            }
            catch (Exception ex)
            {
                _logger.LogError($"Nieoczekiwany błąd podczas wysyłania e-maila do {to}: {ex.Message}");
                throw; // Przekazuje wyjątek dalej
            }
        }
    }
}
