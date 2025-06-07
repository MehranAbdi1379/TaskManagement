using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManagement.Domain.Models;
using TaskManagement.Repository.EntityConfigurations;

namespace TaskManagement.Repository
{
    public class TaskManagementDBContext(DbContextOptions<TaskManagementDBContext> options)
        : IdentityDbContext<ApplicationUser, ApplicationRole, int>(options)
    {
        public DbSet<AppTask> Tasks { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }
        public DbSet<AppTaskUser> TaskUsers { get; set; }
        public DbSet<BaseNotification> Notifications { get; set; }
        public DbSet<TaskAssignmentRequest> TaskAssignmentRequests { get; set; }
        public DbSet<CommentNotification> CommentNotifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
            modelBuilder.ApplyConfiguration(new TaskConfiguration());
            modelBuilder.ApplyConfiguration(new TaskCommentConfiguration());
            modelBuilder.ApplyConfiguration(new AppTaskUserConfiguration());
            modelBuilder.ApplyConfiguration(new TaskAssignmentRequestConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new CommentNotificationConfiguration());
        }
    }
}
