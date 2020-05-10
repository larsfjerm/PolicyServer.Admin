using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PolicyServer.EntityFramework.Entities;
using PolicyServer.EntityFramework.Extensions;

namespace PolicyServer.EntityFramework.DbContexts
{
    public class ConfigurationDbContext : DbContext
    {
        public ConfigurationDbContext(DbContextOptions<ConfigurationDbContext> options)
            : base(options)
        {
        }

        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<PermissionEntity> Permissions { get; set; }
        public DbSet<PolicyEntity> Policies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureContext();

            base.OnModelCreating(modelBuilder);
        }
    }

    public class DbContextFactory : IDesignTimeDbContextFactory<ConfigurationDbContext>
    {
        public ConfigurationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ConfigurationDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=policyserver;Username=postgres;Password=postgres;");

            return new ConfigurationDbContext(optionsBuilder.Options);
        }
    }
}
