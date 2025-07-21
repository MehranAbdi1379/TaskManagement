using FluentAssertions;
using TaskManagement.Domain.Exceptions;
using TaskManagement.Domain.Models;
using TaskStatus = TaskManagement.Domain.Enums.TaskStatus;

public class AppTaskTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldSetProperties()
    {
        // Arrange
        var title = "Test Task";
        var description = "Some description";
        var status = TaskStatus.InProgress;

        // Act
        var task = new AppTask(title, description, status, 0);

        // Assert
        task.Title.Should().Be(title);
        task.Description.Should().Be(description);
        task.Status.Should().Be(status);
    }

    [Fact]
    public void Constructor_WithEmptyTitle_ShouldThrowDomainException()
    {
        // Arrange
        var title = "";
        var description = "Desc";
        var status = TaskStatus.Pending;

        // Act
        Action act = () => new AppTask(title, description, status, 0);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Title of the task can not be empty.");
    }

    [Fact]
    public void SetTitle_WithValidTitle_ShouldUpdateTitle()
    {
        var task = new AppTask("Old Title", "Desc", TaskStatus.Pending, 0);

        task.SetTitle("New Title");

        task.Title.Should().Be("New Title");
    }

    [Fact]
    public void SetTitle_WithEmptyTitle_ShouldThrowDomainException()
    {
        var task = new AppTask("Old Title", "Desc", TaskStatus.Pending, 0);

        var act = () => task.SetTitle("");

        act.Should().Throw<DomainException>()
            .WithMessage("Title of the task can not be empty.");
    }

    [Fact]
    public void SetDescription_ShouldUpdateDescription()
    {
        var task = new AppTask("Title", "Old Desc", TaskStatus.Pending, 0);

        task.SetDescription("New Desc");

        task.Description.Should().Be("New Desc");
    }

    [Fact]
    public void SetStatus_WithValidStatus_ShouldUpdateStatus()
    {
        var task = new AppTask("Title", "Desc", TaskStatus.Pending, 0);

        task.SetStatus(TaskStatus.Completed);

        task.Status.Should().Be(TaskStatus.Completed);

        task.SetStatus(TaskStatus.InProgress);
        task.Status.Should().Be(TaskStatus.InProgress);
    }

    [Fact]
    public void SetStatus_WithInvalidEnumValue_ShouldThrowDomainException()
    {
        var task = new AppTask("Title", "Desc", TaskStatus.Pending, 0);

        var act = () => task.SetStatus((TaskStatus)999);

        act.Should().Throw<DomainException>()
            .WithMessage($"Invalid Task Status Code with code 999 for Task Id {task.Id}");
    }

    [Fact]
    public void SetOwnerId_ShouldSetOwnerId()
    {
        var task = new AppTask("Title", "Desc", TaskStatus.Pending, 0);

        task.SetOwnerId(42);

        task.OwnerId.Should().Be(42);
    }

    [Fact]
    public void Delete_ShouldSetDeletedToTrue()
    {
        var task = new AppTask("Title", "Desc", TaskStatus.Pending, 0);

        task.Delete();

        task.Deleted.Should().BeTrue();
    }

    [Fact]
    public void Delete_WhenAlreadyDeleted_ShouldThrowDomainException()
    {
        var task = new AppTask("Title", "Desc", TaskStatus.Pending, 0);

        task.Delete();

        var act = () => task.Delete();

        act.Should().Throw<DomainException>()
            .WithMessage("Can not undelete an entity");
    }

    [Fact]
    public void UpdateUpdatedAt_ShouldUpdateTimestamp()
    {
        var task = new AppTask("Title", "Desc", TaskStatus.Pending, 0);
        var original = task.UpdatedAt;

        Thread.Sleep(10); // Small delay to ensure timestamp change
        task.UpdateUpdatedAt();

        task.UpdatedAt.Should().BeAfter(original);
    }

    [Fact]
    public void BaseEntity_ShouldSetInitialValues()
    {
        var task = new AppTask("Title", "Desc", TaskStatus.Pending, 0);

        task.Id.Should().Be(0);
        task.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        task.UpdatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        task.Deleted.Should().BeFalse();
    }

    [Fact]
    public void Retrieve_ShouldReturnPropertiesTaskAndDescriptionAndStatusAndOwnerId()
    {
        var task = new AppTask("Title", "Desc", TaskStatus.Pending, 0);

        task.Title.Should().Be("Title");
        task.Description.Should().Be("Desc");
        task.Status.Should().Be(TaskStatus.Pending);
    }

    [Fact]
    public void EmptyConstructor()
    {
        var task = new AppTask();
        task.Title.Should().BeNull();
        task.Description.Should().BeNull();
        task.Status.Should().Be(0);
    }

    [Fact]
    public void Relationships()
    {
        var task = new AppTask("Title", "Desc", TaskStatus.Pending, 0);
        var owner = new ApplicationUser
        {
            Id = 3
        };
        var comments = new List<TaskComment>();
        var assignedUsers = new List<ApplicationUser>();
        var taskAssignmentRequests = new List<TaskAssignmentRequest>();

        task.Comments = comments;
        task.SetOwnerId(3);
        task.Owner = owner;
        task.TaskAssignmentRequests = taskAssignmentRequests;

        task.OwnerId.Should().Be(3);
        task.Owner.Should().Be(owner);
        task.Comments.Should().BeEquivalentTo(comments);
        task.TaskAssignmentRequests.Should().BeEquivalentTo(taskAssignmentRequests);
    }
}