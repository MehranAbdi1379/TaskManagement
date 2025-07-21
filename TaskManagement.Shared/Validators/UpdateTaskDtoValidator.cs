using FluentValidation;
using TaskManagement.Shared.DTOs.Task;

namespace TaskManagement.Shared.Validators
{
    public class UpdateTaskDtoValidator: AbstractValidator<UpdateTaskDto>
    {
        public UpdateTaskDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(300).WithMessage("Description cannot exceed 300 characters.");

            RuleFor(x => (int)x.TaskStatus)
                .InclusiveBetween(0, 3).WithMessage("Status code should be from 0 to 3");
        }
    }
}
