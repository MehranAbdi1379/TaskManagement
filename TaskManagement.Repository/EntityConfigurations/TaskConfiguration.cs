using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domain.Models;

namespace TaskManagement.Repository.EntityConfigurations;

public class TaskConfiguration : IEntityTypeConfiguration<AppTask>
{
    public void Configure(EntityTypeBuilder<AppTask> builder)
    {
        builder.Property(t => t.Deleted).IsRequired();
        builder.Property(t => t.Title).IsRequired().HasColumnType("nvarchar(100)");
        builder.Property(t => t.Description).IsRequired(false).HasColumnType("nvarchar(300)");
        builder.Property(t => t.Status).IsRequired();
        builder.Property(t => t.UpdatedAt).IsRequired();
        builder.Property(t => t.CreatedAt).IsRequired();
        builder.Property(t => t.OwnerId).IsRequired();

        builder.HasOne(t => t.Owner)
            .WithMany() // Or .WithMany(u => u.OwnedTasks)
            .HasForeignKey(t => t.OwnerId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

        builder.HasMany(t => t.AssignedUsers)
            .WithMany(u => u.AssignedTasks)
            .UsingEntity<Dictionary<string, object>>(
                "AppTaskAssignment",
                j => j
                    .HasOne<ApplicationUser>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Restrict), // ❗ prevent cascading deletes here
                j => j
                    .HasOne<AppTask>()
                    .WithMany()
                    .HasForeignKey("TaskId")
                    .OnDelete(DeleteBehavior.Restrict) // ❗ and here
            );
    }
}