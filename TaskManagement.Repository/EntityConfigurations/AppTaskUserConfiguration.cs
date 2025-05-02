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
    public class AppTaskUserConfiguration : IEntityTypeConfiguration<AppTaskUser>
    {
        public void Configure(EntityTypeBuilder<AppTaskUser> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.TaskId).IsRequired();
            builder.HasOne<AppTask>()
               .WithMany()
               .HasForeignKey(x => x.TaskId)
               .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
