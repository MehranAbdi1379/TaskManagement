using FluentAssertions;
using TaskManagement.Domain.Models;

public class TaskAssignmentRequestTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldInitializeProperties()
    {
        // Arrange
        var taskOwnerId = 1;
        var assigneeId = 2;
        var taskId = 3;

        // Act
        var request = new TaskAssignmentRequest(assigneeId, taskId);

        // Assert
        request.AssigneeId.Should().Be(assigneeId);
        request.TaskId.Should().Be(taskId);
    }

    [Fact]
    public void SetRequestNotificationId_ShouldUpdateProperty()
    {
        var request = new TaskAssignmentRequest();

        request.SetRequestNotificationId(99);

        request.RequestNotificationId.Should().Be(99);
    }

    [Fact]
    public void SetAssigneeId_ShouldUpdateProperty()
    {
        var request = new TaskAssignmentRequest();

        request.SetAssigneeId(25);

        request.AssigneeId.Should().Be(25);
    }

    [Fact]
    public void SetTaskId_ShouldUpdateProperty()
    {
        var request = new TaskAssignmentRequest();

        request.SetTaskId(35);

        request.TaskId.Should().Be(35);
    }

    [Fact]
    public void SetIsAccepted_ShouldUpdateProperty()
    {
        var request = new TaskAssignmentRequest();

        request.SetIsAccepted(true);
        request.IsAccepted.Should().BeTrue();

        request.SetIsAccepted(false);
        request.IsAccepted.Should().BeFalse();
    }

    [Fact]
    public void InheritsFromBaseEntity_ShouldHaveBaseProperties()
    {
        var request = new TaskAssignmentRequest();

        request.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        request.UpdatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        request.Id.Should().Be(0);
        request.Deleted.Should().BeFalse();
    }

    [Fact]
    public void Relationships()
    {
        var request = new TaskAssignmentRequest();
        var taskOwner = new ApplicationUser();
        var assignee = new ApplicationUser();
        var task = new AppTask();
        var requestNotification = new BaseNotification();

        request.Assignee = assignee;
        request.Task = task;
        request.RequestNotification = requestNotification;

        request.Assignee.Should().Be(assignee);
        request.Task.Should().Be(task);
        request.RequestNotification.Should().Be(requestNotification);
    }
}