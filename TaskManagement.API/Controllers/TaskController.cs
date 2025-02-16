using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Domain.Models;
using TaskManagement.Service.Interfaces;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        protected readonly ITaskService taskService;
        public TaskController(ITaskService taskService)
        {
            this.taskService = taskService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTasksAsync()
        {
            try
            {
                var tasks = await taskService.GetAllTasksAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskByIdAsync(int id)
        {
            try
            {
                var task = await taskService.GetTaskByIdAsync(id);
                return Ok(task);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddTaskAsync(string title, string? description, string status)
        {
            try
            {
                var task = new AppTask(title,description,status);
                task = await taskService.CreateTaskAsync(task);
                return Ok(task);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTaskAsync(int id ,string? title, string? description, string? status)
        {
            try
            {
                var task = await taskService.GetTaskByIdAsync(id);
                if(!string.IsNullOrEmpty(description)) task.SetDescription(description);
                if (!string.IsNullOrEmpty(status)) task.SetStatus(status);
                if(!string.IsNullOrEmpty(title)) task.SetTitle(title);
                task.UpdatedAt = DateTime.Now;

                task = await taskService.UpdateTaskAsync(task);
                return Ok(task);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTaskAsync(int id)
        {
            try
            {
                await taskService.DeleteTaskByIdAsync(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
    }
}
