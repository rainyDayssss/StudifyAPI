using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using StudifyAPI.Features.FriendRequests.Model;
using StudifyAPI.Features.Friends.Model;
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
        public DbSet<UserTask> UserTasks { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Friend> Friends { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasOne(u => u.Streak)
                .WithOne(us => us.User)
                .HasForeignKey<UserStreak>(us => us.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Tasks)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.SentFriendRequests)
                .WithOne(fr => fr.Sender)
                .HasForeignKey(fr => fr.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<User>()
                .HasMany(u => u.ReceivedFriendRequests)
                .WithOne(fr => fr.Receiver)
                .HasForeignKey(fr => fr.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FriendRequest>()
                .HasIndex(fr => new { fr.SenderId, fr.ReceiverId })
                .IsUnique();


            modelBuilder.Entity<Friend>()
                .HasKey(f => new { f.UserAId, f.UserBId });

            modelBuilder.Entity<Friend>()
                .HasOne(f => f.UserA)
                .WithMany(u => u.FriendsAsUserA)
                .HasForeignKey(u => u.UserAId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friend>()
                .HasOne(f => f.UserB)
                .WithMany(u => u.FriendsAsUserB)
                .HasForeignKey(u => u.UserBId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
