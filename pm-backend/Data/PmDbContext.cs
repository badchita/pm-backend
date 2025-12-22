using Microsoft.EntityFrameworkCore;
using pm_backend.Models;

namespace pm_backend.Data
{
    public class PmDbContext : DbContext
    {
        public PmDbContext(DbContextOptions<PmDbContext> options) : base(options) {}

        public DbSet<User> Users => Set<User>();
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }
        public DbSet<TaskStateHistory> TaskStateHistories { get; set; }
    }
}
