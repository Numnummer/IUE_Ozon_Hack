using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using NotificationService.Abstractions;
using NotificationService.Options;
using NotificationService.ContractModels;

namespace NotificationService.Services
{
    public class EmailService(IOptionsMonitor<MailSettings> mailSettingsMonitor,
        ILogger<EmailService> logger) :
        IEmailService
    {
        public async Task<bool> SendEmailAsync(EmailRequestDto emailRequestDto)
        {
            try
            {
                var mail = new MimeMessage();
                mail.Sender=MailboxAddress.Parse(mailSettingsMonitor.CurrentValue.Sender);
                mail.From.Add(InternetAddress.Parse(mailSettingsMonitor.CurrentValue.Sender));
                mail.Subject=emailRequestDto.subject;
                mail.To.Add(InternetAddress.Parse(emailRequestDto.to));
                var builder = new BodyBuilder();
                builder.TextBody = emailRequestDto.body;
                mail.Body=builder.ToMessageBody();

                using var mailClient = new SmtpClient();
                logger.LogInformation($"{DateTime.UtcNow} Sender {mailSettingsMonitor.CurrentValue.Sender}");
                logger.LogInformation($"{DateTime.UtcNow} Headers {string.Join('\n', mail.Headers)}");
                await mailClient.ConnectAsync(mailSettingsMonitor.CurrentValue.Server,
                    mailSettingsMonitor.CurrentValue.Port);
                await mailClient.AuthenticateAsync(mailSettingsMonitor.CurrentValue.Sender,
                    mailSettingsMonitor.CurrentValue.Password);
                await mailClient.SendAsync(mail);
                await mailClient.DisconnectAsync(true);
                logger.LogInformation($"{DateTime.UtcNow} Сообщение отправлено на почту {emailRequestDto.to}");
                return true;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                logger.LogError(e.StackTrace);
                return false;
            }
        }
    }
}
