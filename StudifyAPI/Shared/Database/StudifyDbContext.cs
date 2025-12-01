using Microsoft.EntityFrameworkCore;
using StudifyAPI.Features.Users.Models;

namespace StudifyAPI.Common.Database
{
    public class StudifyDbContext : DbContext
    {
        public StudifyDbContext(DbContextOptions<StudifyDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserLevel> UserLevels { get; set; }
        public DbSet<UserStreak> UserStreaks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasOne(u => u.Level)
                .WithOne(ul => ul.User)
                .HasForeignKey<UserLevel>(ul => ul.Id);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Streak)
                .WithOne(us => us.User)
                .HasForeignKey<UserStreak>(us => us.Id);
        }
    }
}
