using AutoMapper;
using TaskManagement.Domain.Models;
using TaskManagement.Shared.DTOs.Task;

namespace TaskManagement.Shared.Mappings;

public class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<CreateTaskDto, AppTask>()
            .ConstructUsing(dto => new AppTask(dto.Title, dto.Description, dto.Status));
        CreateMap<AppTask, TaskResponseDto>();
    }
}