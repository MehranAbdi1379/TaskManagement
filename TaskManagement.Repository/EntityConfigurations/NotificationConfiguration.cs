using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domain.Models;

namespace TaskManagement.Repository.EntityConfigurations;

public class NotificationConfiguration : IEntityTypeConfiguration<BaseNotification>
{
    public void Configure(EntityTypeBuilder<BaseNotification> builder)
    {
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.Content).IsRequired().HasColumnType("nvarchar(500)");
        builder.Property(x => x.Title).IsRequired().HasColumnType("nvarchar(200)");
        builder.Property(x => x.IsRead).IsRequired();
        builder
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(n => n.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.UserId);
    }
}