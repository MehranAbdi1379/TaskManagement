using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TaskManagement.API.Hubs;
using TaskManagement.API.Services;
using TaskManagement.Shared.DTOs.Notification;
using TaskManagement.Shared.DTOs.Task;
using TaskManagement.Shared.DTOs.TaskComment;
using TaskManagement.Shared.ServiceInterfaces;

namespace TaskManagement.API.Controllers;

[Route("api/tasks")]
[Authorize]
[ApiController]
public class TaskController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IHubContext<NotificationHub> notificationHub;
    private readonly ITaskCommentService taskCommentService;
    private readonly TaskGroupTracker taskGroupTracker;
    private readonly ITaskService taskService;

    public TaskController(ITaskService taskService, ITaskCommentService taskCommentService,
        IHubContext<NotificationHub> notificationHub
        , TaskGroupTracker taskGroupTracker, IMapper mapper)
    {
        this.taskService = taskService;
        this.taskCommentService = taskCommentService;
        this.notificationHub = notificationHub;
        this.taskGroupTracker = taskGroupTracker;
        this.mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTasksAsync([FromQuery] TaskQueryParameters parameters)
    {
        var tasks = await taskService.GetAllTasksAsync(parameters);
        return Ok(tasks);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTaskByIdAsync(int id)
    {
        var task = await taskService.GetTaskByIdAsync(id);
        return Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> AddTaskAsync(CreateTaskDto dto)
    {
        var task = await taskService.CreateTaskAsync(dto);
        return Ok(task);
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateTaskStateAsync(UpdateTaskStatusDto dto)
    {
        return Ok(await taskService.UpdateTaskStatusAsync(dto));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTaskAsync(UpdateTaskDto dto)
    {
        var task = await taskService.UpdateTaskAsync(dto);
        return Ok(task);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTaskAsync(int id)
    {
        await taskService.DeleteTaskByIdAsync(id);
        return Ok("Task is deleted.");
    }

    [HttpGet("{id}/comment")]
    public async Task<IActionResult> GetTaskCommentsAsync([FromQuery] TaskCommentQueryParameters parameters, int id)
    {
        var taskComments = await taskCommentService.GetTaskCommentsByTaskIdAsync(parameters, id);
        return Ok(taskComments);
    }

    [HttpPost("{id}/comment")]
    public async Task<IActionResult> AddTaskCommentAsync(CreateTaskCommentDto dto, int id)
    {
        var (taskComment, baseNotifications) =
            await taskCommentService.CreateTaskCommentAsync(dto, id, taskGroupTracker.TaskUserGroups);
        var tempIsOwner = taskComment.IsOwner;
        taskComment.IsOwner = false;
        await notificationHub.Clients.Group($"Task-{taskComment.TaskId}").SendAsync("ReceiveTaskComment", taskComment);
        taskComment.IsOwner = tempIsOwner;
        foreach (var notification in baseNotifications)
        {
            var notificationResponseDto = mapper.Map<NotificationResponseDto>(notification);
            notificationResponseDto.Type = notification.Type.ToString();

            await notificationHub.Clients.User(notification.UserId.ToString())
                .SendAsync("ReceiveNotification", notificationResponseDto);
        }

        return Ok(taskComment);
    }

    [HttpDelete("{id}/comment/{taskCommentId}")]
    public async Task<IActionResult> DeleteTaskCommentAsync(int taskCommentId)
    {
        var taskComment = await taskCommentService.DeleteTaskCommentByIdAsync(taskCommentId);
        await notificationHub.Clients.Group($"Task-{taskComment.TaskId}")
            .SendAsync("DeleteTaskComment", taskComment.Id);
        return Ok("Task comment is deleted.");
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AssignUserToTask([FromBody] TaskAssignmentRequestDto request)
    {
        var notification = await taskService.RequestTaskAssignmentAsync(request.AssigneeEmail, request.TaskId);

        var dto = mapper.Map<NotificationResponseDto>(notification);

        await notificationHub.Clients.User(notification.UserId.ToString()).SendAsync("ReceiveNotification", dto);

        return Ok("Task invitation sent.");
    }

    [HttpPost("respond")]
    public async Task<IActionResult> RespondToAssignment([FromBody] TaskAssignmentResponseDto request)
    {
        var notification =
            await taskService.RespondToTaskAssignmentAsync(request.RequestNotificationId, request.Accept);

        var dto = mapper.Map<NotificationResponseDto>(notification);

        await notificationHub.Clients.User(notification.UserId.ToString()).SendAsync("ReceiveNotification", dto);

        return Ok("Response recorded.");
    }

    [HttpGet("{id}/assigned-users")]
    public async Task<IActionResult> GetAssignedUsers(int id)
    {
        var result = await taskService.GetTaskAssignedUsers(id);
        return Ok(result);
    }

    [HttpDelete("{id}/unassign-user/{userId}")]
    public async Task<IActionResult> UnassignUserFromTask(int id, int userId)
    {
        var notification = await taskService.UnassignTaskAsync(id, userId);

        var dto = mapper.Map<NotificationResponseDto>(notification);

        await notificationHub.Clients.User(notification.UserId.ToString()).SendAsync("ReceiveNotification", dto);

        return Ok($"User with id {userId} has been unassigned from the task {id}.");
    }
}