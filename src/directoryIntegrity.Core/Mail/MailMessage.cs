using System;
using System.Collections.Generic;
using System.Text;

namespace directoryIntegrity.Core.Mail
{
    public class MailMessage
    {
        public MailAddress Recipient { get; set; }
        public MailAddress Sender { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
    }

    public class MailAddress
    {
        public string DisplayName { get; set; }
        public string Address { get; set; }

        public MailAddress(string address)
        {
            Address = address;
        }

        public MailAddress(string displayName, string address)
        {
            DisplayName = displayName;
            Address = address;
        }
    }
}