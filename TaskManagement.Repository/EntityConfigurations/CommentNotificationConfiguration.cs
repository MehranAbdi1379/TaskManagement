using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domain.Models;

namespace TaskManagement.Repository.EntityConfigurations;

public class CommentNotificationConfiguration: IEntityTypeConfiguration<CommentNotification>
{
    public void Configure(EntityTypeBuilder<CommentNotification> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CommentId).IsRequired();
        builder.Property(x => x.NotificationId).IsRequired();
        builder.HasOne<TaskComment>()
            .WithMany()
            .HasForeignKey(x => x.CommentId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<BaseNotification>()
            .WithMany()
            .HasForeignKey(x => x.NotificationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}