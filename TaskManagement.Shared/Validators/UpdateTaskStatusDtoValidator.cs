using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Service.DTOs.Task;

namespace TaskManagement.Service.Validators
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
