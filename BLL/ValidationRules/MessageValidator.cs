using BE.Concrete;
using FluentValidation;

namespace BLL.ValidationRules
{
    public class MessageValidator : AbstractValidator<Message>
    {
        public MessageValidator()
        {
            RuleFor(x => x.ReceiverId).NotNull().GreaterThan(0).WithMessage("Bitte wählen Sie einen Empfänger.");
            RuleFor(x => x.Subject).NotEmpty().WithMessage("Bitte geben Sie einen Betreff ein.")
                .MaximumLength(120).WithMessage("Der Betreff darf höchstens 120 Zeichen lang sein.");
            RuleFor(x => x.Details).NotEmpty().WithMessage("Bitte schreiben Sie eine Nachricht.")
                .MaximumLength(3000).WithMessage("Die Nachricht darf höchstens 3.000 Zeichen lang sein.");
        }
    }
}
