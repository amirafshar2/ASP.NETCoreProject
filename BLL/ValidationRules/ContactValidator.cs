using BE.Concrete;
using FluentValidation;

namespace BLL.ValidationRules
{
    public class ContactValidator : AbstractValidator<Contact>
    {
        public ContactValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Bitte geben Sie Ihren Namen ein.")
                .MaximumLength(60).WithMessage("Der Name darf höchstens 60 Zeichen lang sein.");
            RuleFor(x => x.Mail).NotEmpty().WithMessage("Bitte geben Sie Ihre E-Mail-Adresse ein.")
                .EmailAddress().WithMessage("Bitte geben Sie eine gültige E-Mail-Adresse ein.");
            RuleFor(x => x.Subject).NotEmpty().WithMessage("Bitte geben Sie einen Betreff ein.")
                .MaximumLength(120).WithMessage("Der Betreff darf höchstens 120 Zeichen lang sein.");
            RuleFor(x => x.Message).NotEmpty().WithMessage("Bitte schreiben Sie eine Nachricht.")
                .MinimumLength(10).WithMessage("Die Nachricht muss mindestens 10 Zeichen lang sein.")
                .MaximumLength(3000).WithMessage("Die Nachricht darf höchstens 3.000 Zeichen lang sein.");
        }
    }
}
