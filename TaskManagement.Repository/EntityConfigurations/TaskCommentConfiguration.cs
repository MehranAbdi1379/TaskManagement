using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models;

namespace TaskManagement.Repository.EntityConfigurations
{
    public class TaskCommentConfiguration : IEntityTypeConfiguration<TaskComment>
    {
        public void Configure(EntityTypeBuilder<TaskComment> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Text).IsRequired().HasColumnType("nvarchar(500)");
            builder.Property(x => x.TaskId).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
            builder.HasOne<AppTask>()
               .WithMany()
               .HasForeignKey(x => x.TaskId);
        }
    }
}
