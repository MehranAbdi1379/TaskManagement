using Microsoft.AspNetCore.SignalR;
using TaskManagement.Shared.DTOs.Notification;
using TaskManagement.Shared.DTOs.TaskComment;

namespace TaskManagement.API.Hubs
{
    public class NotificationHub : Hub
    {
        // Join a task group (users viewing a specific task will join)
        public async Task JoinTaskGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        // Leave a task group (when a user navigates away)
        public async Task LeaveTaskGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
