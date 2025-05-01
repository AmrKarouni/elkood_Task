using ElKood.Application.Shared.Models.Inputs;
using FluentValidation;

namespace elkood_Task.Validators
{
    public class ToDoTaskInputValidator : AbstractValidator<ToDoTaskInput>
    {
        public ToDoTaskInputValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("To-Do Task Name Is Required.")
                .MinimumLength(3).WithMessage("To-Do Task Name Minimum Length Is 3.")
                .MaximumLength(20).WithMessage("To-Do Task Name Maximum Length Is 20.");

            RuleFor(x => x.Priority)
                .InclusiveBetween(1, 5).WithMessage("Priority Must between 1 & 5.");

            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("CategoryId is required.");
        }
    }
}
