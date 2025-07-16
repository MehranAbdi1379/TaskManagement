using FluentAssertions;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Exceptions;
using TaskManagement.Domain.Models;

public class BaseNotificationTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldInitializeCorrectly()
    {
        // Arrange
        var userId = 10;
        var title = "New Comment";
        var content = "You have a new comment.";
        var type = NotificationType.NewTaskComment;

        // Act
        var notification = new BaseNotification(userId, title, content, type);

        // Assert
        notification.UserId.Should().Be(userId);
        notification.Title.Should().Be(title);
        notification.Content.Should().Be(content);
        notification.Type.Should().Be(type);
        notification.IsRead.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void SetTitle_WithInvalidTitle_ShouldThrowDomainException(string invalidTitle)
    {
        var notification = new BaseNotification();

        var act = () => notification.SetTitle(invalidTitle);

        act.Should().Throw<DomainException>()
            .WithMessage("Title cannot be null or empty.");
    }

    [Fact]
    public void SetTitle_WithValidTitle_ShouldUpdateTitle()
    {
        var notification = new BaseNotification();

        notification.SetTitle("New Like");

        notification.Title.Should().Be("New Like");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void SetContent_WithInvalidContent_ShouldThrowDomainException(string invalidContent)
    {
        var notification = new BaseNotification();

        var act = () => notification.SetContent(invalidContent);

        act.Should().Throw<DomainException>()
            .WithMessage("Content cannot be null or empty.");
    }

    [Fact]
    public void SetContent_WithValidContent_ShouldUpdateContent()
    {
        var notification = new BaseNotification();

        notification.SetContent("You were mentioned in a task.");

        notification.Content.Should().Be("You were mentioned in a task.");
    }

    [Fact]
    public void SetType_WithInvalidEnum_ShouldThrowDomainException()
    {
        var notification = new BaseNotification();

        var act = () => notification.SetType((NotificationType)999);

        act.Should().Throw<DomainException>()
            .WithMessage("Invalid notification type.");
    }

    [Fact]
    public void SetType_WithValidEnum_ShouldUpdateType()
    {
        var notification = new BaseNotification();

        notification.SetType(NotificationType.General);

        notification.Type.Should().Be(NotificationType.General);
    }

    [Fact]
    public void SetUserId_ShouldUpdateUserId()
    {
        var notification = new BaseNotification();

        notification.SetUserId(42);

        notification.UserId.Should().Be(42);
    }

    [Fact]
    public void ReadNotification_ShouldSetIsReadToTrue()
    {
        var notification = new BaseNotification();

        notification.ReadNotification();

        notification.IsRead.Should().BeTrue();
    }

    [Fact]
    public void ReadNotification_SetIsReadToFalse_ShouldThrowDomainException()
    {
        var notification = new BaseNotification();

        notification.ReadNotification();

        var act = () => notification.ReadNotification();

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void InheritsFromBaseEntity_ShouldHaveBaseProperties()
    {
        var notification = new BaseNotification();

        notification.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        notification.UpdatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        notification.Id.Should().Be(0);
        notification.Deleted.Should().BeFalse();
    }

    [Fact]
    public void Relationships()
    {
        var notification = new BaseNotification();
        var comments = new List<TaskComment>();
        var user = new ApplicationUser();

        notification.TaskComments = comments;
        notification.User = user;

        notification.TaskComments.Should().BeEquivalentTo(comments);
        notification.User.Should().Be(user);
    }
}