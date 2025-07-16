using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using TaskManagement.Domain.Exceptions;
using TaskManagement.Domain.Models;

public class ApplicationUserTests
{
    [Fact]
    public void SetFirstName_WithValidName_ShouldSetFirstName()
    {
        // Arrange
        var user = new ApplicationUser();

        // Act
        user.SetFirstName("Mehi");

        // Assert
        user.FirstName.Should().Be("Mehi");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void SetFirstName_WithInvalidName_ShouldThrowDomainException(string invalidName)
    {
        // Arrange
        var user = new ApplicationUser();

        // Act
        var act = () => user.SetFirstName(invalidName);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("First name cannot be null or empty.");
    }

    [Fact]
    public void SetLastName_WithValidName_ShouldSetLastName()
    {
        var user = new ApplicationUser();

        user.SetLastName("Developer");

        user.LastName.Should().Be("Developer");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void SetLastName_WithInvalidName_ShouldThrowDomainException(string invalidName)
    {
        var user = new ApplicationUser();

        var act = () => user.SetLastName(invalidName);

        act.Should().Throw<DomainException>()
            .WithMessage("Last name cannot be null or empty.");
    }

    [Fact]
    public void PhoneNumber_ShouldBeSettableAndGettable()
    {
        var user = new ApplicationUser();

        user.PhoneNumber = "+989123456789";

        user.PhoneNumber.Should().Be("+989123456789");
    }

    [Fact]
    public void IdentityUserProperties_ShouldBeSettable()
    {
        var user = new ApplicationUser();

        user.UserName = "mehi123";
        user.Email = "mehi@example.com";

        user.UserName.Should().Be("mehi123");
        user.Email.Should().Be("mehi@example.com");
    }

    [Fact]
    public void ApplicationUser_ShouldInheritFromIdentityUser()
    {
        var user = new ApplicationUser();

        user.Should().BeAssignableTo<IdentityUser<int>>();
    }

    [Fact]
    public void Relationships()
    {
        var user = new ApplicationUser();
        var notifications = new List<BaseNotification>();
        var assignedTasks = new List<AppTask>();

        user.Notifications = notifications;

        user.Notifications.Should().BeEquivalentTo(notifications);
        user.AssignedTaskRequests.Should().BeEquivalentTo(assignedTasks);
    }
}