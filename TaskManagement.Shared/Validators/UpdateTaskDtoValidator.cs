using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Service.DTOs.Task;

namespace TaskManagement.Service.Validators
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

            RuleFor(x => (int)x.Status)
                .InclusiveBetween(0, 3).WithMessage("Status code should be from 0 to 3");
        }
    }
}
