using System;
using System.Text;
using directoryIntegrity.Core.Mail;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace directoryIntegrity.Notification
{
    public class Mailer : ISmtpClient
    {
        private readonly MailConfiguration _mailConfiguration;
        public readonly Encoding DefaultEncoding = Encoding.UTF8;

        public Mailer(MailConfiguration mailConfiguration)
        {
            _mailConfiguration = mailConfiguration;
        }

        public void SendMail(MailMessage msg)
        {
            var message = CreateMimeMessage(msg);

            using (var client = new SmtpClient())
            {
                client.Connect(_mailConfiguration.Host, _mailConfiguration.Port, _mailConfiguration.UseSsl);

                // Note: only needed if the SMTP server requires authentication
                if (_mailConfiguration.HasAuthInfo)
                    client.Authenticate(_mailConfiguration.SmtpUser, _mailConfiguration.SmtpPassword);

                client.Send(message);
                client.Disconnect(true);
            }
        }

        private MimeMessage CreateMimeMessage(MailMessage msg)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(DefaultEncoding, msg.Sender.DisplayName ?? "", msg.Sender.Address));

            foreach (var recipient in msg.Recipients)
            {
                message.To.Add(new MailboxAddress(DefaultEncoding, recipient.DisplayName ?? "", recipient.Address));
            }
            
            message.Subject = msg.Subject;
            var textFormat = msg.IsHtml ? TextFormat.Html : TextFormat.Text;
            message.Body = new TextPart(textFormat)
            {
                Text = msg.Body
            };
            return message;
        }
    }
}