using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using directoryIntegrity.Core.Mail;
using Microsoft.Extensions.Configuration;

namespace directoryIntegrity.ConsoleApp
{
    public class MailHelper
    {
        public MailConfiguration CreateMailConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var appSettings = builder.Build();

            var mailCfg = new MailConfiguration();

            appSettings.GetSection("App:Mail").Bind(mailCfg);

            return mailCfg;
        }
    }
}