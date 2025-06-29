using AutoMapper;
using TaskManagement.Domain.Models;
using TaskManagement.Shared.DTOs.Task;

namespace TaskManagement.Shared.Mappings;

public class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<AppTask, TaskResponseDto>();
    }
}