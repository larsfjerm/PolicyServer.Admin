using Microsoft.EntityFrameworkCore;
using PolicyServer.EntityFramework.Entities;
using PolicyServer.EntityFramework.Extensions;

namespace PolicyServer.EntityFramework.DbContexts
{
    public class ConfigurationDbContext : DbContext
    {
        public ConfigurationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Policy> Policies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureContext();

            base.OnModelCreating(modelBuilder);
        }
    }
}
