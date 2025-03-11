using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Models;
using TaskManagement.Service.DTOs.Task;
using TaskManagement.Service.Interfaces;
using TaskManagement.Service.Services;
using TaskManagement.Shared.DTOs.Task;
using TaskManagement.Shared.DTOs.TaskComment;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TaskController : ControllerBase
    {
        protected readonly ITaskService taskService;
        protected readonly ITaskCommentService taskCommentService;
        public TaskController(ITaskService taskService, ITaskCommentService taskCommentService)
        {
            this.taskService = taskService;
            this.taskCommentService = taskCommentService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTasksAsync([FromQuery] TaskQueryParameters parameters)
        {
            var tasks = await taskService.GetAllTasksAsync(parameters);
            return Ok(tasks);
        }

        [HttpGet("{id}")]
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
            var taskComment = await taskCommentService.CreateTaskCommentAsync(dto, id);
            return Ok(taskComment);
        }

        [HttpDelete("{id}/comment")]
        public async Task<IActionResult> DeleteTaskCommentAsync(int id)
        {
            await taskCommentService.DeleteTaskCommentByIdAsync(id);
            return Ok("Task comment is deleted.");
        }

        //[HttpPost("{id}/asign-user")]
        //public async Task<IActionResult> AsignUserToTask(int userId, int id)
        //{
        //    await taskService.AsignTaskToUserAsync(userId, id);
        //    return Ok($"Task {id} is asigned to the user {userId}");
        //}

        [HttpPost("assign")]
        public async Task<IActionResult> AssignUserToTask([FromBody] TaskAssignmentRequestDto request)
        {
            await taskService.RequestTaskAssignmentAsync(request.AssigneeEmail, request.TaskId);

            return Ok("Task invitation sent.");
        }

        [HttpPost("respond")]
        public async Task<IActionResult> RespondToAssignment([FromBody] TaskAssignmentResponseDto request)
        {
            await taskService.RespondToTaskAssignmentAsync(request.RequestNotificationId, request.Accept);
            return Ok("Response recorded.");
        }
    }
}
