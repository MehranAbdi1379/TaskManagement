using AutoMapper;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Models;
using TaskManagement.Shared.DTOs;
using TaskManagement.Shared.DTOs.Notification;
using TaskManagement.Shared.ServiceInterfaces;

namespace TaskManagement.Service.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository baseRepository;
    private readonly IMapper mapper;
    private readonly IUserContext userContext;


    public NotificationService(INotificationRepository baseRepository, IUserContext userContext, IMapper mapper)
    {
        this.baseRepository = baseRepository;
        this.userContext = userContext;
        this.mapper = mapper;
    }

    public async Task<BaseNotification> CreateNotification(int userId, string title, string content,
        NotificationType notificationType)
    {
        var notification = new BaseNotification(userId, title, content, notificationType);

        return await baseRepository.AddAsync(notification);
    }

    public async Task UpdateNotificationIsReadAsync(int notificationId)
    {
        var notification = await baseRepository.GetByIdAsync(notificationId);
        if (notification.UserId != userContext.UserId)
            throw new Exception(
                $"Notification with id {notificationId} does not belong to the user with id {userContext.UserId}.");

        notification.ReadNotification();

        await baseRepository.UpdateAsync(notification);
    }

    public async Task<PagedResult<NotificationResponseDto>> GetUserNotificationsAsync(
        NotificationQueryParameters parameters, bool history)
    {
        var (notifications, totalCount) =
            await baseRepository.GetNotificationsByUserId(parameters.PageNumber, parameters.PageSize,
                parameters.SortOrder, history);

        var mappedResult = mapper.Map<List<NotificationResponseDto>>(notifications);

        return new PagedResult<NotificationResponseDto>
        {
            Items = mappedResult,
            TotalCount = totalCount,
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize
        };
    }
}