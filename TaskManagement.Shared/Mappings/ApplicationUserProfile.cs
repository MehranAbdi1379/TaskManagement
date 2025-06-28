using AutoMapper;
using TaskManagement.Domain.Models;
using TaskManagement.Shared.DTOs.Task;

namespace TaskManagement.Shared.Mappings;

public class ApplicationUserProfile: Profile
{
    public ApplicationUserProfile()
    {
        CreateMap<ApplicationUser, TaskAssignedUserResponseDto>()
            .ForMember(dto => dto.UserId, x => x.MapFrom(u => u.Id));
    }
}