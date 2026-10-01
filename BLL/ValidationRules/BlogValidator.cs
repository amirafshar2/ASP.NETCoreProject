using BE.Concrete;
using FluentValidation;

namespace BLL.ValidationRules
{
    public class BlogValidator : AbstractValidator<Blog>
    {
        public BlogValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Bitte geben Sie einen Titel ein.")
                .MinimumLength(5).WithMessage("Der Titel muss mindestens 5 Zeichen lang sein.")
                .MaximumLength(150).WithMessage("Der Titel darf höchstens 150 Zeichen lang sein.");
            RuleFor(x => x.Summary).NotEmpty().WithMessage("Bitte geben Sie einen kurzen Teaser ein.")
                .MaximumLength(300).WithMessage("Der Teaser darf höchstens 300 Zeichen lang sein.");
            RuleFor(x => x.Content).NotEmpty().WithMessage("Bitte schreiben Sie den Inhalt des Beitrags.")
                .MinimumLength(200).WithMessage("Der Inhalt muss mindestens 200 Zeichen lang sein.")
                .MaximumLength(20000).WithMessage("Der Inhalt darf höchstens 20.000 Zeichen lang sein.");
            RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("Bitte wählen Sie eine Kategorie.");
        }
    }
}
