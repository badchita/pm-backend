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
        public DbSet<TaskCommentReaction> TaskCommentReactions { get; set; }
        public DbSet<Company> Companies{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------------- FIX CASCADE ----------------

            // TaskComment → User: NO ACTION
            modelBuilder.Entity<TaskComment>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // TaskCommentReaction → TaskComment: CASCADE
            modelBuilder.Entity<TaskCommentReaction>()
                .HasOne(r => r.TaskComment)
                .WithMany(c => c.Reactions)
                .HasForeignKey(r => r.TaskCommentId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            // TaskCommentReaction → User: NO ACTION
            modelBuilder.Entity<TaskCommentReaction>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
        }
    }
}
