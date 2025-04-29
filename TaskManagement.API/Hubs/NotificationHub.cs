using System.Text.RegularExpressions;
using Microsoft.AspNetCore.SignalR;
using TaskManagement.API.Services;
using TaskManagement.Shared.DTOs.Notification;
using TaskManagement.Shared.DTOs.TaskComment;
using TaskManagement.Shared.ServiceInterfaces;

namespace TaskManagement.API.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly TaskGroupTracker _taskGroupTracker;
        private readonly IUserContext _userContext;

        public NotificationHub(TaskGroupTracker taskGroupTracker, IUserContext userContext)
        {
            _taskGroupTracker = taskGroupTracker;
            _userContext = userContext;
        }
        // Join a task group (users viewing a specific task will join)
        public async Task JoinTaskGroup(string groupName)
        {
            _taskGroupTracker.AddTaskUserGroup(_userContext.UserId, int.Parse(Regex.Match(groupName, @"\d+").Value));
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        // Leave a task group (when a user navigates away)
        public async Task LeaveTaskGroup(string groupName)
        {
            _taskGroupTracker.RemoveTaskUserGroup(_userContext.UserId, int.Parse(Regex.Match(groupName, @"\d+").Value));
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
