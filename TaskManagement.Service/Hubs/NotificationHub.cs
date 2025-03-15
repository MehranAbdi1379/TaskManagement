using Microsoft.AspNetCore.SignalR;
using TaskManagement.Domain.Models;
using TaskManagement.Shared.DTOs.Notification;
using TaskManagement.Shared.DTOs.TaskComment;

namespace TaskManagement.Service.Hubs
{
    public class NotificationHub : Hub
    {
        // Send notification to all users
        public async Task SendNotification(NotificationResponseDto notification)
        {
            await Clients.All.SendAsync("ReceiveNotification", notification);
        }

        // Send task comment to only users in a specific task group
        public async Task SendTaskComment(int taskId, TaskCommentResponseDto comment)
        {
            await Clients.Group($"task-{taskId}").SendAsync("ReceiveTaskComment", comment);
        }

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
