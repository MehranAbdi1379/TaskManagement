using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models;
using TaskManagement.Service.DTOs.Task;

namespace TaskManagement.Service.Mappings
{
    public class TaskProfile: Profile
    {
        public TaskProfile()
        {
            CreateMap<CreateTaskDto, AppTask>()
                .ConstructUsing(dto => new AppTask(dto.Title, dto.Description, dto.Status));
            CreateMap<AppTask, TaskResponseDto>();
        }
    }
}
