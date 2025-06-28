using FluentValidation;
using TaskManagement.Shared.DTOs.Task;

namespace TaskManagement.Shared.Validators
{
    public class UpdateTaskStatusDtoValidator: AbstractValidator<UpdateTaskStatusDto>
    {
        public UpdateTaskStatusDtoValidator()
        {
            RuleFor(x => (int)x.Status)
               .InclusiveBetween(0, 3).WithMessage("Status code should be from 0 to 3");
        }
    }
}
