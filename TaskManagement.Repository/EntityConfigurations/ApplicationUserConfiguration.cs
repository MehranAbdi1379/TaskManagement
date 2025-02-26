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
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasIndex(t => t.Id);
            builder.Property(t => t.FirstName).IsRequired().HasColumnType("nvarchar(100)");
            builder.Property(t => t.LastName).IsRequired().HasColumnType("nvarchar(100)");
            builder.Property(t => t.PhoneNumber).IsRequired(false).HasColumnType("nvarchar(20)");
        }
    }
}
