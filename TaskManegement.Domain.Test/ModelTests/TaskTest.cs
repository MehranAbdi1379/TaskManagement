using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManegement.Domain.Test.ModelTests
{
    using System;
    using TaskManagement.Domain.Exceptions;
    using TaskManagement.Domain.Models;
    using Xunit;

    namespace YourProject.Domain.Tests
    {
        public class AppTaskTests
        {
            [Fact]
            public void Constructor_Should_CreateTask_When_ValidDataIsProvided()
            {
                // Arrange & Act
                var task = new AppTask("Test Task", "This is a test description.", "Pending");

                // Assert
                Assert.Equal("Test Task", task.Title);
                Assert.Equal("This is a test description.", task.Description);
                Assert.Equal("Pending", task.Status);
            }

            [Fact]
            public void Constructor_Should_ThrowDomainException_When_TitleIsEmpty()
            {
                // Act & Assert
                var exception = Assert.Throws<DomainException>(() => new AppTask("", "Description", "Pending"));
                Assert.Equal("Title of the task can not be empty.", exception.Message);
            }

            [Fact]
            public void Constructor_Should_ThrowDomainException_When_StatusIsEmpty()
            {
                // Act & Assert
                var exception = Assert.Throws<DomainException>(() => new AppTask("Task", "Description", ""));
                Assert.Equal("Status of the task can not be empty.", exception.Message);
            }

            [Fact]
            public void SetTitle_Should_UpdateTitle_When_ValidTitleIsProvided()
            {
                // Arrange
                var task = new AppTask("Old Title", "Description", "Pending");

                // Act
                task.SetTitle("New Title");

                // Assert
                Assert.Equal("New Title", task.Title);
            }

            [Fact]
            public void SetTitle_Should_ThrowDomainException_When_TitleIsEmpty()
            {
                // Arrange
                var task = new AppTask("Old Title", "Description", "Pending");

                // Act & Assert
                var exception = Assert.Throws<DomainException>(() => task.SetTitle(""));
                Assert.Equal("Title of the task can not be empty.", exception.Message);
            }

            [Fact]
            public void SetStatus_Should_UpdateStatus_When_ValidStatusIsProvided()
            {
                // Arrange
                var task = new AppTask("Task", "Description", "Pending");

                // Act
                task.SetStatus("Completed");

                // Assert
                Assert.Equal("Completed", task.Status);
            }

            [Fact]
            public void SetStatus_Should_ThrowDomainException_When_StatusIsEmpty()
            {
                // Arrange
                var task = new AppTask("Task", "Description", "Pending");

                // Act & Assert
                var exception = Assert.Throws<DomainException>(() => task.SetStatus(""));
                Assert.Equal("Status of the task can not be empty.", exception.Message);
            }

            [Fact]
            public void SetDescription_Should_UpdateDescription()
            {
                // Arrange
                var task = new AppTask("Task", "Old Description", "Pending");

                // Act
                task.SetDescription("New Description");

                // Assert
                Assert.Equal("New Description", task.Description);
            }
        }
    }

}
