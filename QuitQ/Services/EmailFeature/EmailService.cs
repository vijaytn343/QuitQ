using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using QuitQ.Configurations;
using MailKit.Net.Smtp;

namespace QuitQ.Services.EmailFeature
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(
            IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendEmailAsync(
            string toEmail,
            string subject,
            string body)
        {
            var email = new MimeMessage();

            email.From.Add(
                new MailboxAddress(
                    _settings.SenderName,
                    _settings.SenderEmail));

            email.To.Add(
                MailboxAddress.Parse(toEmail));

            email.Subject = subject;

            email.Body =
                new TextPart("html")
                {
                    Text = body
                };

            using var smtp = new SmtpClient();

            try
            {
                await smtp.ConnectAsync(
                    _settings.SmtpServer,
                    _settings.Port,
                    SecureSocketOptions.Auto);

                await smtp.AuthenticateAsync(
     _settings.Username,
     _settings.Password);

                await smtp.SendAsync(email);

                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("EMAIL ERROR:");
                Console.WriteLine(ex.ToString());

                throw;
            }
        }
    }
}
