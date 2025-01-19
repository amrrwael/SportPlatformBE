using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlatformSport.Models;

namespace PlatformSport.Database
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Add DbSets for Sport and Stadium
        public DbSet<Sport> Sports { get; set; }
        public DbSet<Stadium> Stadiums { get; set; }
    }
}
