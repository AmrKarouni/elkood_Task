using ElKood.Application.Shared.Models.Inputs.Identity;
using FluentValidation;

namespace elkood_Task.Validators
{
    public class LoginInputValidator : AbstractValidator<LoginInput>
    {
        public LoginInputValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");

        }
    }
}
