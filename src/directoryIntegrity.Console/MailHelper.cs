using System;
using System.IO;
using System.Linq;
using directoryIntegrity.Core.Mail;
using directoryIntegrity.Notification;
using Microsoft.Extensions.Configuration;

namespace directoryIntegrity.ConsoleApp
{
    public class MailHelper
    {
        private static string _settingsFile;

        /// <summary>
        /// Creates a mail configuration from the data specified in appsettings.json
        /// </summary>
        public static MailConfiguration CreateMailConfiguration()
        {
            _settingsFile = "appsettings.json";
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(_settingsFile);

            var appSettings = builder.Build();

            var mailCfg = new MailConfiguration();

            appSettings.GetSection("App:Mail").Bind(mailCfg);

            if (string.IsNullOrEmpty(mailCfg.SmtpPassword))
            {
                Console.Write($"No SMTP password found in {_settingsFile}. Please specify: ");
                mailCfg.SmtpPassword = Console.ReadLine();
            }

            return mailCfg;
        }

        internal static void SendReportByMailDontThrow(string mailBody, ScanOptions opts)
        {
            try
            {
                SendReportByMail(mailBody, opts);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        internal static void SendReportByMail(string mailBody, ScanOptions opts)
        {
            var mailCfg = CreateMailConfiguration();

            var mailer = new Mailer(mailCfg);
            var msg = new MailMessage
            {
                Sender = new MailAddress(mailCfg.Sender),
                Recipients = mailCfg.Recipients.Select(m => new MailAddress(m)).ToList(),
                Subject = $"Directory scan of {opts.DirectoryToScan}",
                Body = mailBody
            };

            mailer.SendMail(msg);
        }
    }
}