using AutoMapper;
using TaskManagement.Domain.Models;
using TaskManagement.Shared.DTOs.Notification;

namespace TaskManagement.Shared.Mappings;

public class NotificationProfile : Profile
{
    public NotificationProfile()
    {
        CreateMap<BaseNotification, NotificationResponseDto>();
    }
}