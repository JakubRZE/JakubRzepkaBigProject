using MailService.Entities;

namespace MailService.Interfaces
{
    public interface ISMTPService
    {
        public void SendMail(Mail mail);
    }
}