using BE.Concrete;
using FluentValidation;

namespace BLL.ValidationRules
{
    public class NotificationValidator : AbstractValidator<Notification>
    {
        public NotificationValidator()
        {
            RuleFor(x => x.Type).NotEmpty().WithMessage("Bitte geben Sie einen Titel ein.")
                .MaximumLength(60).WithMessage("Der Titel darf höchstens 60 Zeichen lang sein.");
            RuleFor(x => x.Details).NotEmpty().WithMessage("Bitte geben Sie einen Text ein.")
                .MaximumLength(500).WithMessage("Der Text darf höchstens 500 Zeichen lang sein.");
        }
    }
}
