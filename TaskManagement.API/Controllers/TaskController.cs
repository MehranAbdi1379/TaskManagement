using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Models;
using TaskManagement.Service.DTOs.Task;
using TaskManagement.Service.Interfaces;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TaskController : ControllerBase
    {
        protected readonly ITaskService taskService;
        public TaskController(ITaskService taskService)
        {
            this.taskService = taskService;
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
    }
}
