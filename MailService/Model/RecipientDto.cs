using FluentValidation;

namespace MailService.Model
{
    public class RecipientDto
    {
        public string RecipientMail { get; set; }
    }

    public class RecipientValidator : AbstractValidator<RecipientDto>
    {
        public RecipientValidator()
        {
            RuleFor(x => x.RecipientMail).EmailAddress().WithMessage("Invalid email address"); ;
        }
    }
}