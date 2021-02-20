using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using YAFF.Core.Interfaces;
using YAFF.Core.Settings;

namespace YAFF.Business.Common
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _settings;

        public EmailSender(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendEmail(string to, string subject, string body, bool htmlBody = false)
        {
            var email = BuildEmail(to, subject, body, htmlBody);

            using var client = new SmtpClient();

            await client.ConnectAsync(_settings.Host, _settings.Port);
            await client.AuthenticateAsync(_settings.UserName, _settings.Password);

            await client.SendAsync(email);

            await client.DisconnectAsync(true);
        }


        private MimeMessage BuildEmail(string to, string subject, string body, bool htmlBody)
        {
            var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress("Yaff Inc.", _settings.UserName));
            msg.To.Add(new MailboxAddress(to, to));
            msg.Subject = subject;
            msg.Body = new TextPart(htmlBody ? TextFormat.Html : TextFormat.Text)
            {
                Text = body
            };
            return msg;
        }
    }
}