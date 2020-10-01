using FluentValidation;
using MailService.Entities;
using MailService.Enumerations;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MailService.Model
{
    public class MailDto
    {
        public MailDto()
        {
        }

        public MailDto(Mail mail)
        {
            UserId = mail.UserId;
            Sender = mail.Sender;
            Content = mail.Content;
            Priority = mail.Priority;
            Status = mail.Status;
            Recipients = mail.Recipients.Select(mailRecipient => new RecipientDto
            {
                RecipientMail = mailRecipient.RecipientMail
            }).ToList();
        }

        public int UserId { get; set; }
        public string Sender { get; set; }
        public string Content { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
        public ICollection<RecipientDto> Recipients { get; set; } = new Collection<RecipientDto>();
    }

    public class MailValidator : AbstractValidator<MailDto>
    {
        public MailValidator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty();
            RuleFor(x => x.Sender).EmailAddress();
            RuleForEach(x => x.Recipients).NotEmpty().WithMessage("At least one recipient is required");
        }
    }
}