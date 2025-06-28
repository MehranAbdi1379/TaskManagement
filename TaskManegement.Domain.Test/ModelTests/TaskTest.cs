using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Exceptions;
using TaskManagement.Domain.Models;

namespace TaskManegement.Domain.Test.ModelTests
{
    namespace YourProject.Domain.Tests
    {
        public class AppTaskTests
        {
            [Fact]
            public void Constructor_Should_CreateTask_When_ValidDataIsProvided()
            {
                // Arrange & Act
                var task = new AppTask("Test Task", "This is a test description.", Status.Pending);

                // Assert
                Assert.Equal("Test Task", task.Title);
                Assert.Equal("This is a test description.", task.Description);
                Assert.Equal(Status.Pending, task.Status);
            }

            [Fact]
            public void Constructor_Should_ThrowDomainException_When_TitleIsEmpty()
            {
                // Act & Assert
                var exception = Assert.Throws<DomainException>(() => new AppTask("", "Description", Status.Pending));
                Assert.Equal("Title of the task can not be empty.", exception.Message);
            }

            [Fact]
            public void SetTitle_Should_UpdateTitle_When_ValidTitleIsProvided()
            {
                // Arrange
                var task = new AppTask("Old Title", "Description", Status.Pending);

                // Act
                task.SetTitle("New Title");

                // Assert
                Assert.Equal("New Title", task.Title);
            }

            [Fact]
            public void SetTitle_Should_ThrowDomainException_When_TitleIsEmpty()
            {
                // Arrange
                var task = new AppTask("Old Title", "Description", Status.Pending);

                // Act & Assert
                var exception = Assert.Throws<DomainException>(() => task.SetTitle(""));
                Assert.Equal("Title of the task can not be empty.", exception.Message);
            }

            [Fact]
            public void SetStatus_Should_UpdateStatus_When_ValidStatusIsProvided()
            {
                // Arrange
                var task = new AppTask("Task", "Description", Status.Pending);

                // Act
                task.SetStatus(Status.Completed);

                // Assert
                Assert.Equal(Status.Completed, task.Status);
            }

            [Fact]
            public void SetDescription_Should_UpdateDescription()
            {
                // Arrange
                var task = new AppTask("Task", "Old Description", Status.Pending);

                // Act
                task.SetDescription("New Description");

                // Assert
                Assert.Equal("New Description", task.Description);
            }

            [Fact]
            public void SetTitle_ShouldThrow_WhenTitleIsEmpty()
            {
                // Arrange
                var task = new AppTask("Valid Title", "Description", Status.Pending);

                // Act & Assert
                var ex = Assert.Throws<DomainException>(() => task.SetTitle(""));
                Assert.Equal("Title of the task can not be empty.", ex.Message);
            }

            [Theory]
            [InlineData(Status.Pending)]
            [InlineData(Status.InProgress)]
            [InlineData(Status.Completed)]
            [InlineData(Status.Cancelled)]
            public void SetStatus_ShouldUpdateStatus(Status newStatus)
            {
                // Arrange
                var task = new AppTask("Task", "Description", Status.Pending);

                // Act
                task.SetStatus(newStatus);

                // Assert
                Assert.Equal(newStatus, task.Status);
            }

            [Theory]
            [InlineData(-1)]
            [InlineData(4)]
            public void SetStatus_ShouldThrow_WhenStatusIsInvalid(int invalidStatus)
            {
                // Arrange
                var task = new AppTask("Task", "Description", Status.Pending);

                // Act & Assert
                var ex = Assert.Throws<DomainException>(() => task.SetStatus((Status)invalidStatus));
                Assert.Equal("Task Status Code should be between 0 and 3", ex.Message);
            }
        }
    }

}
