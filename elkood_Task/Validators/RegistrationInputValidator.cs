using ElKood.Application.Shared.Models.Inputs.Identity;
using FluentValidation;

namespace elkood_Task.Validators
{
    public class RegistrationInputValidator : AbstractValidator<RegistrationInput>
    {
        public RegistrationInputValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Must be a valid email");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required.");
        }
    }
}
