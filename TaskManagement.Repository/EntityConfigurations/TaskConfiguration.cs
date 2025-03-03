using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models;

namespace TaskManagement.Repository.EntityConfigurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<AppTask>
    {
        public void Configure(EntityTypeBuilder<AppTask> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Deleted).IsRequired();
            builder.Property(t => t.Title).IsRequired().HasColumnType("nvarchar(100)");
            builder.Property(t => t.Description).IsRequired(false).HasColumnType("nvarchar(300)");
            builder.Property(t => t.Status).IsRequired();
            builder.Property(t => t.UpdatedAt).IsRequired();
            builder.Property(t => t.CreatedAt).IsRequired();
            builder.Property(t => t.OwnerId).IsRequired();
        }
    }
}
