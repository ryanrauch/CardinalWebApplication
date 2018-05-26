using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CardinalWebApplication.Models;
using CardinalWebApplication.Models.DbContext;

namespace CardinalWebApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<CurrentLayer>()
                   .HasKey(c => c.UserId);
            builder.Entity<FriendRequest>()
                   .HasKey(f => new { f.InitiatorId, f.TargetId });

            builder.Entity<Zone>()
                   .HasKey(z => z.ZoneID);
            builder.Entity<ZoneShape>()
                   .HasKey(z => z.ZoneShapeID);
        }
        public DbSet<ApplicationOption> ApplicationOptions { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<CurrentLayer> CurrentLayers { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<LocationHistory> LocationHistories { get; set; }

        public DbSet<Zone> Zones { get; set; }
        public DbSet<ZoneShape> ZoneShapes { get; set; }
    }
}
