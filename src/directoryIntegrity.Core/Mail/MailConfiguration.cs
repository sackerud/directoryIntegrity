namespace directoryIntegrity.Core.Mail
{
    public class MailConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; } = 587;
        public bool UseSsl { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPassword { get; set; }
        public bool HasAuthInfo => !string.IsNullOrEmpty(SmtpUser) && !string.IsNullOrEmpty(SmtpPassword);
    }
}