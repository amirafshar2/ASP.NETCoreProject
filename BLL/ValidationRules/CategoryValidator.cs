using BE.Concrete;
using FluentValidation;

namespace BLL.ValidationRules
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Bitte geben Sie einen Namen ein.")
                .MaximumLength(60).WithMessage("Der Name darf höchstens 60 Zeichen lang sein.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Bitte geben Sie eine Beschreibung ein.")
                .MaximumLength(300).WithMessage("Die Beschreibung darf höchstens 300 Zeichen lang sein.");
            RuleFor(x => x.Color).Matches("^#[0-9A-Fa-f]{6}$").WithMessage("Bitte wählen Sie eine gültige Farbe.");
        }
    }
}
