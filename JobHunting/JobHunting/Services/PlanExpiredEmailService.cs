using System.Net.Mail;
using System.Net;

namespace JobHunting.Services
{
    public class PlanExpiredEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromEmail;

        public PlanExpiredEmailService(IConfiguration configuration)
        {
            _smtpClient = new SmtpClient
            {
                Host = configuration["Smtp:Host"],
                Port = int.Parse(configuration["Smtp:Port"]),
                EnableSsl = true,
                Credentials = new NetworkCredential(configuration["Smtp:Username"], configuration["Smtp:Password"])
            };

            _fromEmail = configuration["Smtp:FromEmail"];
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var mailMessage = new MailMessage(_fromEmail, toEmail, subject, body)
            {
                IsBodyHtml = true // 啟用HTML支持
            };

            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
