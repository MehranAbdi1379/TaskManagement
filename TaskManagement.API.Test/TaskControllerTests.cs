using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using FluentAssertions;
using TaskManagement.API.Test.MyApi;
using Microsoft.AspNetCore.Mvc.Testing;
using TaskManagement.Domain.Enums;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using TaskManagement.Service.DTOs;
using TaskManagement.Service.DTOs.Task;

namespace TaskManagement.API.Test
{
    namespace MyApi.Tests
    {
        public class TaskControllerTests : IClassFixture<WebApplicationFactory<Program>>
        {
            private readonly HttpClient _client;
            private readonly WebApplicationFactory<Program> _factory;

            public TaskControllerTests(WebApplicationFactory<Program> factory)
            {
                _factory = factory;
                _client = factory.CreateClient();
            }

            [Fact]
            public async Task GetAllTasksAsync_ReturnsOkResult_WithTasks()
            {
                // Arrange
                var queryParams = "?pageNumber=1&pageSize=10&sortOrder=asc"; // Add filters if necessary
                var requestUri = $"/api/task{queryParams}";

                // Act
                var response = await _client.GetAsync(requestUri);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);

                var result = JsonConvert.DeserializeObject<PagedResult<TaskResponseDto>>(await response.Content.ReadAsStringAsync());
                result.Items.Should().NotBeEmpty();
            }

            [Fact]
            public async Task GetTaskByIdAsync_ReturnsOkResult_WithTask()
            {
                // Arrange
                int taskId = 1; // Example task ID
                var requestUri = $"/api/task/{taskId}";

                // Act
                var response = await _client.GetAsync(requestUri);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);

                var task = JsonConvert.DeserializeObject<TaskResponseDto>(await response.Content.ReadAsStringAsync());
                task.Should().NotBeNull();
                task.Id.Should().Be(taskId);
            }

            [Fact]
            public async Task AddTaskAsync_ReturnsCreatedResult_WithTask()
            {
                // Arrange
                var createTaskDto = new CreateTaskDto
                {
                    Title = "Test Task",
                    Description = "Test task description",
                    Status = Status.Pending
                };
                var content = new StringContent(JsonConvert.SerializeObject(createTaskDto), System.Text.Encoding.UTF8, "application/json");

                // Act
                var response = await _client.PostAsync("/api/task", content);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);

                var task = JsonConvert.DeserializeObject<TaskResponseDto>(await response.Content.ReadAsStringAsync());
                task.Should().NotBeNull();
                task.Title.Should().Be(createTaskDto.Title);
                task.Description.Should().Be(createTaskDto.Description);
                task.Status.Should().Be(createTaskDto.Status);
            }

            [Fact]
            public async Task UpdateTaskStateAsync_ReturnsOkResult_WithUpdatedTask()
            {
                // Arrange
                var updateTaskStatusDto = new UpdateTaskStatusDto
                {
                    Id = 1, // Example task ID
                    Status = Status.Completed
                };
                var content = new StringContent(JsonConvert.SerializeObject(updateTaskStatusDto), System.Text.Encoding.UTF8, "application/json");

                // Act
                var response = await _client.PatchAsync("/api/task", content);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);

                var task = JsonConvert.DeserializeObject<TaskResponseDto>(await response.Content.ReadAsStringAsync());
                task.Status.Should().Be(Status.Completed);
            }

            [Fact]
            public async Task UpdateTaskAsync_ReturnsOkResult_WithUpdatedTask()
            {
                // Arrange
                var updateTaskDto = new UpdateTaskDto
                {
                    Id = 1, // Example task ID
                    Title = "Updated Task Title",
                    Description = "Updated task description",
                    Status = Status.InProgress
                };
                var content = new StringContent(JsonConvert.SerializeObject(updateTaskDto), System.Text.Encoding.UTF8, "application/json");

                // Act
                var response = await _client.PutAsync("/api/task", content);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);

                var task = JsonConvert.DeserializeObject<TaskResponseDto>(await response.Content.ReadAsStringAsync());
                task.Title.Should().Be(updateTaskDto.Title);
                task.Description.Should().Be(updateTaskDto.Description);
                task.Status.Should().Be(updateTaskDto.Status);
            }

            [Fact]
            public async Task DeleteTaskAsync_ReturnsOkResult()
            {
                // Arrange
                int taskId = 220; // Example task ID
                var requestUri = $"/api/task/{taskId}";

                // Act
                var response = await _client.DeleteAsync(requestUri);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }
    }

}