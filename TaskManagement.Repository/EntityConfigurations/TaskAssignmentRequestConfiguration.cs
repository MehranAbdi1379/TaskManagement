using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domain.Models;

namespace TaskManagement.Repository.EntityConfigurations;

public class TaskAssignmentRequestConfiguration : IEntityTypeConfiguration<TaskAssignmentRequest>
{
    public void Configure(EntityTypeBuilder<TaskAssignmentRequest> builder)
    {
        builder.Property(x => x.IsAccepted).IsRequired();
        builder.Property(x => x.RequestNotificationId);
    }
}