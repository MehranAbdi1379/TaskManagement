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
    public class NotificationConfiguration : IEntityTypeConfiguration<BaseNotification>
    {
        public void Configure(EntityTypeBuilder<BaseNotification> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.Content).IsRequired().HasColumnType("nvarchar(500)");
            builder.Property(x => x.Title).IsRequired().HasColumnType("nvarchar(200)");
            builder.Property(x => x.IsRead).IsRequired();
            builder.HasOne<ApplicationUser>()
               .WithMany()
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
