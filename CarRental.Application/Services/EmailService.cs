using System.Net;
using System.Net.Mail;
using CarRental.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CarRental.Application.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;
    private readonly ISmtpClient _smtpClient;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger,
        ISmtpClient smtpClient)
    {
        _configuration = configuration;
        _logger = logger;
        _smtpClient = smtpClient;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var senderName = _configuration["EmailSettings:SenderName"];
            
            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail!, senderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(to);

            _logger.LogInformation($"Sending email to {to} with subject '{subject}'.");

            await _smtpClient.SendMailAsync(mailMessage);

            _logger.LogInformation($"Email to {to} was successfully sent.");

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
