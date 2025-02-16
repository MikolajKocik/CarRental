using System.Net;
using System.Net.Mail;
using CarRental.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CarRental.Application.Services;

public class EmailService : IEmailService
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
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]!);
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var senderName = _configuration["EmailSettings:SenderName"];
            var senderPassword = _configuration["EmailSettings:SenderPassword"];

            using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                smtpClient.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail!, senderName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(to);

                _logger.LogInformation($"Sending email to {to} with subject '{subject}'.");

                await smtpClient.SendMailAsync(mailMessage);

                _logger.LogInformation($"Email to {to} was successfully sent.");
            }
        }
        catch (SmtpException smtpEx)
        {
            _logger.LogError($"SMTP error while sending email to {to}: {smtpEx.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Unexpected error while sending email to {to}: {ex.Message}");
            throw;
        }
    }
}
