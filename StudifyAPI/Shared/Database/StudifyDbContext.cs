using Microsoft.EntityFrameworkCore;
using StudifyAPI.Features.Tasks.Model;
using StudifyAPI.Features.Users.Models;
using StudifyAPI.Features.UserStreaks.Model;

namespace StudifyAPI.Shared.Database
{
    public class StudifyDbContext : DbContext
    {
        public StudifyDbContext(DbContextOptions<StudifyDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserStreak> UserStreaks { get; set; }
        public DbSet<UserTaskCreateDTO> UserTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasOne(u => u.Streak)
                .WithOne(us => us.User)
                .HasForeignKey<UserStreak>(us => us.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Tasks)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId);
        }
    }
}
