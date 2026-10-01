using BE.Concrete;
using FluentValidation;

namespace BLL.ValidationRules
{
    public class WriterValidator : AbstractValidator<Writer>
    {
        public WriterValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Bitte geben Sie einen Namen ein.")
                .MinimumLength(2).WithMessage("Der Name muss mindestens 2 Zeichen lang sein.")
                .MaximumLength(50).WithMessage("Der Name darf höchstens 50 Zeichen lang sein.");
            RuleFor(x => x.About).MaximumLength(600).WithMessage("Die Beschreibung darf höchstens 600 Zeichen lang sein.");
        }
    }
}
