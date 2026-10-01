using BE.Concrete;
using FluentValidation;

namespace BLL.ValidationRules
{
    public class AboutValidator : AbstractValidator<About>
    {
        public AboutValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Bitte geben Sie eine Überschrift ein.");
            RuleFor(x => x.Details1).NotEmpty().WithMessage("Bitte geben Sie den ersten Textabschnitt ein.");
        }
    }
}
