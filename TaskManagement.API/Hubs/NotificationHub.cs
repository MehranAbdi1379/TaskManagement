using Microsoft.AspNetCore.SignalR;
using TaskManagement.Shared.DTOs.Notification;
using TaskManagement.Shared.DTOs.TaskComment;

namespace TaskManagement.API.Hubs
{
    public class NotificationHub : Hub
    {
        // Join a task group (users viewing a specific task will join)
        public async Task JoinTaskGroup(int taskId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"task-{taskId}");
        }

        // Leave a task group (when a user navigates away)
        public async Task LeaveTaskGroup(int taskId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"task-{taskId}");
        }
    }
}
