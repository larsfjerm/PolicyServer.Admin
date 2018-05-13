using Microsoft.EntityFrameworkCore;
using PolicyServer.EntityFramework.Entities;

namespace PolicyServer.EntityFramework.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ConfigureContext(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permission>(permission =>
            {
                permission.ToTable("Permission");
                permission.HasKey(x => x.Id);

                permission.Property(x => x.Name).HasMaxLength(200).IsRequired();
                permission.Property(x => x.Description).HasMaxLength(1000);
                permission.Property(x => x.DisplayName).HasMaxLength(200);
                permission.Property(x => x.Enabled).IsRequired();

                permission.HasIndex(x => x.Name).IsUnique();

                permission.HasMany(x => x.PermissionRoles);
            });

            modelBuilder.Entity<Role>(role =>
            {
                role.ToTable("Role");
                role.HasKey(x => x.Id);

                role.Property(x => x.Name).HasMaxLength(200).IsRequired();
                role.Property(x => x.Description).HasMaxLength(1000);
                role.Property(x => x.DisplayName).HasMaxLength(200);
                role.Property(x => x.Enabled).IsRequired();

                role.HasIndex(x => x.Name).IsUnique();

                role.HasMany(x => x.RoleSubjects);
            });

            modelBuilder.Entity<Policy>(policy =>
            {
                policy.ToTable("Policy");
                policy.HasKey(x => x.Id);

                policy.Property(x => x.Name).HasMaxLength(200).IsRequired();
                policy.Property(x => x.Description).HasMaxLength(1000);
                policy.Property(x => x.DisplayName).HasMaxLength(200);
                policy.Property(x => x.Enabled).IsRequired();

                policy.HasIndex(x => x.Name).IsUnique();

                policy.HasMany(x => x.PolicyRoles);
                policy.HasMany(x => x.PolicyPermissions);
            });

            modelBuilder.Entity<RoleSubject>(roleSubject => { roleSubject.HasKey(x => new {x.RoleId, x.Subject}); });
            modelBuilder.Entity<PermissionRole>(permissionRole => { permissionRole.HasKey(x => new {x.PermissionId, x.RoleId}); });
            modelBuilder.Entity<PolicyRole>(policyRole => { policyRole.HasKey(x => new {x.PolicyId, x.RoleId }); });
            modelBuilder.Entity<PolicyPermission>(policyPermission => { policyPermission.HasKey(x => new {x.PolicyId, x.PermissionId }); });
        }
    }
}
