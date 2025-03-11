using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models;

namespace TaskManagement.Repository.EntityConfigurations
{
    public class TaskAssignmentRequestConfiguration : IEntityTypeConfiguration<TaskAssignmentRequest>
    {
        public void Configure(EntityTypeBuilder<TaskAssignmentRequest> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.IsAccepted).IsRequired();
            builder.Property(x => x.RequestNotificationId);
            builder.HasOne<ApplicationUser>()
               .WithMany()
               .HasForeignKey(x => x.AssigneeId)
               .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne<ApplicationUser>()
               .WithMany()
               .HasForeignKey(x => x.TaskOwnerId)
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
