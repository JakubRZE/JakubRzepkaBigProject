using MailService.Enumerations;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MailService.Entities
{
    public class Mail
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Sender { get; set; }
        public string Content { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
        public virtual ICollection<Recipient> Recipients { get; set; } = new Collection<Recipient>();
        public int NumberOfTries { get; set; }
    }
}

