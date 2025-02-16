using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManagement.Domain.Models;
using TaskManagement.Repository.EntityConfigurations;

namespace TaskManagement.Repository
{
    public class TaskManagementDBContext: DbContext
    {
        public TaskManagementDBContext(DbContextOptions<TaskManagementDBContext> options) : base(options) { }

        public DbSet<AppTask> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new TaskConfiguration());
        }
    }
}
