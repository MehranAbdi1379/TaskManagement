using FluentAssertions;
using TaskManagement.Domain.Exceptions;
using TaskManagement.Domain.Models;

public class TaskCommentTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldInitializeProperties()
    {
        // Arrange
        var taskId = 10;
        var userId = 20;
        var text = "This is a comment.";

        // Act
        var comment = new TaskComment(taskId, userId, text);

        // Assert
        comment.TaskId.Should().Be(taskId);
        comment.UserId.Should().Be(userId);
        comment.Text.Should().Be(text);
    }

    [Fact]
    public void SetTaskId_ShouldUpdateTaskId()
    {
        var comment = new TaskComment();

        comment.SetTaskId(5);

        comment.TaskId.Should().Be(5);
    }

    [Fact]
    public void SetUserId_ShouldUpdateUserId()
    {
        var comment = new TaskComment();

        comment.SetUserId(15);

        comment.UserId.Should().Be(15);
    }

    [Fact]
    public void SetText_WithValidText_ShouldUpdateText()
    {
        var comment = new TaskComment();

        comment.SetText("Valid comment");

        comment.Text.Should().Be("Valid comment");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void SetText_WithNullOrEmptyText_ShouldThrowDomainException(string invalidText)
    {
        var comment = new TaskComment();

        var act = () => comment.SetText(invalidText);

        act.Should().Throw<DomainException>()
            .WithMessage("TaskComment text can not be empty.");
    }

    [Fact]
    public void InheritsFromBaseEntity_ShouldHaveBaseProperties()
    {
        var comment = new TaskComment();

        comment.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        comment.UpdatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        comment.Id.Should().Be(0);
        comment.Deleted.Should().BeFalse();
    }

    [Fact]
    public void Relationships()
    {
        var comment = new TaskComment();
        var notifications = new List<BaseNotification>();
        var user = new ApplicationUser();
        var task = new AppTask();

        comment.Notifications = notifications;
        comment.User = user;
        comment.Task = task;

        comment.Notifications.Should().BeEquivalentTo(notifications);
        comment.User.Should().Be(user);
        comment.Task.Should().Be(task);
    }
}