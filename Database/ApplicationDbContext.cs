// ApplicationDbContext.cs
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlatformSport.Models;

namespace PlatformSport.Database
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Sport> Sports { get; set; }
        public DbSet<Stadium> Stadiums { get; set; }
        public DbSet<Room> Rooms { get; set; }  // Add this line

        // ApplicationDbContext.cs
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure many-to-many relationship between Room and User
            modelBuilder.Entity<Room>()
                .HasMany(r => r.Players)
                .WithMany()
                .UsingEntity(j => j.ToTable("RoomPlayers"));  // Creates a join table for Room and User

            // Configure relationship between Room and Sport
            modelBuilder.Entity<Room>()
                .HasOne(r => r.Sport)
                .WithMany()
                .HasForeignKey(r => r.SportId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascade delete

            // Configure relationship between Room and Stadium


            // Configure relationship between Room and Host User
            modelBuilder.Entity<Room>()
                .HasOne(r => r.HostUser)
                .WithMany()
                .HasForeignKey(r => r.HostUserId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascade delete
            modelBuilder.Entity<Room>()
        .HasOne(r => r.Sport)
        .WithMany()
        .HasForeignKey(r => r.SportId)
        .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // Configure relationship between Room and Stadium
            modelBuilder.Entity<Room>()
                .HasOne(r => r.Stadium)
                .WithMany()
                .HasForeignKey(r => r.StadiumId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

          
        }
    }
}
