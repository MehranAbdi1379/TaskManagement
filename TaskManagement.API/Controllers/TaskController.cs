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

    [HttpGet("{taskId:int}")]
    public async Task<IActionResult> GetTaskByIdAsync(int taskId)
    {
        var task = await taskService.GetTaskByIdAsync(taskId);
        return Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> AddTaskAsync(CreateTaskDto dto)
    {
        var task = await taskService.CreateTaskAsync(dto);
        return Ok(task);
    }

    [HttpPatch("{taskId:int}")]
    public async Task<IActionResult> UpdateTaskStateAsync(UpdateTaskStatusDto dto, int taskId)
    {
        return Ok(await taskService.UpdateTaskStatusAsync(dto, taskId));
    }

    [HttpPut("{taskId:int}")]
    public async Task<IActionResult> UpdateTaskAsync(int taskId, UpdateTaskDto dto)
    {
        var task = await taskService.UpdateTaskAsync(dto, taskId);
        return Ok(task);
    }

    [HttpDelete("{taskId:int}")]
    public async Task<IActionResult> DeleteTaskAsync(int taskId)
    {
        await taskService.DeleteTaskByIdAsync(taskId);
        return Ok("Task is deleted.");
    }

    [HttpGet("{taskId:int}/comments")]
    public async Task<IActionResult> GetTaskCommentsAsync([FromQuery] TaskCommentQueryParameters parameters, int taskId)
    {
        var taskComments = await taskCommentService.GetTaskCommentsByTaskIdAsync(parameters, taskId);
        return Ok(taskComments);
    }

    [HttpPost("{taskId:int}/comments")]
    public async Task<IActionResult> AddTaskCommentAsync(CreateTaskCommentDto dto, int taskId)
    {
        var (taskComment, baseNotifications) =
            await taskCommentService.CreateTaskCommentAsync(dto, taskId, taskGroupTracker.TaskUserGroups);
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

    [HttpDelete("{taskId:int}/comments/{commentId:int}")]
    public async Task<IActionResult> DeleteTaskCommentAsync(int commentId, int taskId)
    {
        var taskComment = await taskCommentService.DeleteTaskCommentByIdAsync(commentId);
        await notificationHub.Clients.Group($"Task-{taskComment.TaskId}")
            .SendAsync("DeleteTaskComment", taskComment.Id);
        return Ok("Task comment is deleted.");
    }

    [HttpPost("{taskId:int}/assignees")]
    public async Task<IActionResult> AssignUserToTask(int taskId, [FromBody] TaskAssignmentRequestDto request)
    {
        var notification = await taskService.RequestTaskAssignmentAsync(request.AssigneeEmail, taskId);

        var dto = mapper.Map<NotificationResponseDto>(notification);

        await notificationHub.Clients.User(notification.UserId.ToString()).SendAsync("ReceiveNotification", dto);

        return Ok("Task invitation sent.");
    }

    [HttpPost("{taskId:int}/assignees/{requestNotificationId:int}")]
    public async Task<IActionResult> RespondToAssignment(int taskId, int requestNotificationId,
        [FromBody] TaskAssignmentResponseDto request)
    {
        var notification =
            await taskService.RespondToTaskAssignmentAsync(requestNotificationId, request.Accept);

        var dto = mapper.Map<NotificationResponseDto>(notification);

        await notificationHub.Clients.User(notification.UserId.ToString()).SendAsync("ReceiveNotification", dto);

        return Ok("Response recorded.");
    }

    [HttpGet("{taskId:int}/assignees")]
    public async Task<IActionResult> GetAssignedUsers(int taskId)
    {
        var result = await taskService.GetTaskAssignedUsers(taskId);
        return Ok(result);
    }

    [HttpDelete("{taskId:int}/assignees/{userId:int}")]
    public async Task<IActionResult> UnassignUserFromTask(int taskId, int userId)
    {
        var notification = await taskService.UnassignTaskAsync(taskId, userId);

        var dto = mapper.Map<NotificationResponseDto>(notification);

        await notificationHub.Clients.User(notification.UserId.ToString()).SendAsync("ReceiveNotification", dto);

        return Ok($"User with id {userId} has been unassigned from the task {taskId}.");
    }

    [HttpGet("report")]
    public async Task<IActionResult> GetReport([FromQuery] TaskReportQueryDto dto)
    {
        var result = await taskService.GetTaskReport(dto);
        return Ok(result);
    }
}