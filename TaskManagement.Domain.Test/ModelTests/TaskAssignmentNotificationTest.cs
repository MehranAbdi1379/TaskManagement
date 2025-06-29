using FluentAssertions;
using TaskManagement.Domain.Models;

public class TaskAssignmentNotificationTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldInitializeProperties()
    {
        // Arrange
        var notificationId = 101;
        var accepted = true;
        var requestId = 202;

        // Act
        var taskAssignmentNotification = new TaskAssignmentNotification(notificationId, accepted, requestId);

        // Assert
        taskAssignmentNotification.NotificationId.Should().Be(notificationId);
        taskAssignmentNotification.Accepted.Should().Be(accepted);
        taskAssignmentNotification.RequestId.Should().Be(requestId);
    }

    [Fact]
    public void SetAccepted_ShouldUpdateAcceptedProperty()
    {
        var notification = new TaskAssignmentNotification();

        notification.SetAccepted(true);
        notification.Accepted.Should().BeTrue();

        notification.SetAccepted(false);
        notification.Accepted.Should().BeFalse();
    }

    [Fact]
    public void SetRequestId_ShouldUpdateRequestIdProperty()
    {
        var notification = new TaskAssignmentNotification();

        notification.SetRequestId(50);
        notification.RequestId.Should().Be(50);
    }

    [Fact]
    public void SetNotificationId_ShouldUpdateNotificationIdProperty()
    {
        var notification = new TaskAssignmentNotification();

        notification.SetNotificationId(77);
        notification.NotificationId.Should().Be(77);
    }

    [Fact]
    public void InheritsFromBaseEntity_ShouldHaveBaseProperties()
    {
        var notification = new TaskAssignmentNotification();

        notification.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        notification.UpdatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        notification.Id.Should().Be(0);
        notification.Deleted.Should().BeFalse();
    }

    [Fact]
    public void Relationships()
    {
        var taskAssignmentNotification = new TaskAssignmentNotification();
        var notification = new BaseNotification();
        var request = new TaskAssignmentRequest();

        taskAssignmentNotification.Notification = notification;
        taskAssignmentNotification.Request = request;

        taskAssignmentNotification.Notification.Should().Be(notification);
        taskAssignmentNotification.Request.Should().Be(request);
    }
}