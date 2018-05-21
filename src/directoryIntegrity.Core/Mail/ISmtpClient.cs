namespace directoryIntegrity.Core.Mail
{
    public interface ISmtpClient
    {
        void SendMail(MailMessage msg);
    }
}