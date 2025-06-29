using FluentAssertions;
using TaskManagement.Domain.Exceptions;
using TaskManagement.Domain.Models;

public class TestEntity : BaseEntity
{
}

public class BaseEntityTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Act
        var entity = new TestEntity();

        // Assert
        entity.Id.Should().Be(0);
        entity.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        entity.UpdatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        entity.Deleted.Should().BeFalse();
    }

    [Fact]
    public void Delete_WhenNotDeleted_ShouldMarkAsDeleted()
    {
        var entity = new TestEntity();

        entity.Delete();

        entity.Deleted.Should().BeTrue();
    }

    [Fact]
    public void Delete_WhenAlreadyDeleted_ShouldThrowDomainException()
    {
        var entity = new TestEntity();
        entity.Delete(); // First deletion

        var act = () => entity.Delete(); // Second deletion should fail

        act.Should().Throw<DomainException>()
            .WithMessage("Can not undelete an entity");
    }

    [Fact]
    public void UpdateUpdatedAt_ShouldChangeUpdatedAt()
    {
        var entity = new TestEntity();
        var original = entity.UpdatedAt;

        Thread.Sleep(10); // Ensure the clock ticks

        entity.UpdateUpdatedAt();

        entity.UpdatedAt.Should().BeAfter(original);
    }

    [Fact]
    public void Delete_ShouldNotChangeCreatedAtOrUpdatedAt()
    {
        var entity = new TestEntity();
        var originalCreatedAt = entity.CreatedAt;
        var originalUpdatedAt = entity.UpdatedAt;

        Thread.Sleep(10); // Simulate delay
        entity.Delete();

        entity.CreatedAt.Should().Be(originalCreatedAt);
        entity.UpdatedAt.Should().Be(originalUpdatedAt);
    }
}