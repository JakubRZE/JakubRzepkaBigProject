namespace MailService.Entities
{
    public class Recipient
    {
        public int Id { get; set; }
        public string RecipientMail { get; set; }
        public virtual Mail Mail { get; set; }
    }
}