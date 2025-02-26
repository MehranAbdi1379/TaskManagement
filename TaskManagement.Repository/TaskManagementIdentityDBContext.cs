using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Models;
using TaskManagement.Repository.EntityConfigurations;

namespace TaskManagement.Repository
{
    public class TaskManagementIdentityDBContext: IdentityDbContext<ApplicationUser>
    {
        public TaskManagementIdentityDBContext(DbContextOptions<TaskManagementIdentityDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
        }
    }
}
