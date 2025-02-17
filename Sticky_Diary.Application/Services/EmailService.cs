using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Sticky_Dairy.Application.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789@#$!";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Sticky Diary", emailSettings["SenderEmail"]));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(emailSettings["SmtpServer"], int.Parse(emailSettings["Port"]), SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(emailSettings["SenderEmail"], emailSettings["SenderPassword"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
