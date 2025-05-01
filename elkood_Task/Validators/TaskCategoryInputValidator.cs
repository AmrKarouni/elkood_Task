using ElKood.Application.Shared.Models.Inputs;
using FluentValidation;

namespace elkood_Task.Validators
{
    public class TaskCategoryInputValidator : AbstractValidator<TaskCategoryInput>
    {
        public TaskCategoryInputValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Task Category Name Is Required.")
                .MinimumLength(3).WithMessage("Task Category Name Minimum Length Is 3.")
                .MaximumLength(10).WithMessage("Task Category Name Maximum Length Is 10.");
        }
    }
}
