using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using CarRental.Domain.Interfaces;

namespace CarRental.Application.Services.Tests
{
    public class EmailServiceTests
    {

        [Fact()]
        public async void SendEmailAsync_ShouldSendEmail_IfUserExists()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            var mockLogger = new Mock<ILogger<EmailService>>();
            var mockSmtpClient = new Mock<ISmtpClient>();

            mockConfiguration.Setup(config => config["EmailSettings:SmtpServer"]).Returns("smtp.example.com");
            mockConfiguration.Setup(config => config["EmailSettings:SmtpPort"]).Returns("587");
            mockConfiguration.Setup(config => config["EmailSettings:SenderEmail"]).Returns("email@example.com");
            mockConfiguration.Setup(config => config["EmailSettings:SenderName"]).Returns("car rental");
            mockConfiguration.Setup(config => config["EmailSettings:SenderPassword"]).Returns("password");

            var emailService = new EmailService(mockConfiguration.Object, mockLogger.Object, mockSmtpClient.Object);

            // Act
            await emailService.SendEmailAsync("user@gmail.com", "subject", "body");

            // Assert
            mockSmtpClient.Verify(client => client.SendMailAsync(It.IsAny<MailMessage>()), Times.Once);
        }
    }
}