using BE.Concrete;
using FluentValidation;

namespace BLL.ValidationRules
{
    public class CommentValidator : AbstractValidator<Comment>
    {
        public CommentValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Bitte geben Sie Ihren Namen ein.")
                .MaximumLength(60).WithMessage("Der Name darf höchstens 60 Zeichen lang sein.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Bitte geben Sie Ihre E-Mail-Adresse ein.")
                .EmailAddress().WithMessage("Bitte geben Sie eine gültige E-Mail-Adresse ein.");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Bitte geben Sie einen Betreff ein.")
                .MaximumLength(100).WithMessage("Der Betreff darf höchstens 100 Zeichen lang sein.");
            RuleFor(x => x.Content).NotEmpty().WithMessage("Bitte schreiben Sie einen Kommentar.")
                .MinimumLength(10).WithMessage("Der Kommentar muss mindestens 10 Zeichen lang sein.")
                .MaximumLength(1500).WithMessage("Der Kommentar darf höchstens 1.500 Zeichen lang sein.");
            RuleFor(x => x.Score).InclusiveBetween(1, 5).WithMessage("Bitte vergeben Sie 1 bis 5 Sterne.");
        }
    }
}
