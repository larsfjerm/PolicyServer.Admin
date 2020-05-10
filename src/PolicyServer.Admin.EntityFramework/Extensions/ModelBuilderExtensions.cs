using Microsoft.EntityFrameworkCore;
using PolicyServer.EntityFramework.Entities;

namespace PolicyServer.EntityFramework.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ConfigureContext(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PermissionEntity>(permission =>
            {
                permission.ToTable("permission");
                permission.HasKey(x => x.Id);

                permission.Property(x => x.Name).HasMaxLength(200).IsRequired();
                permission.Property(x => x.Description).HasMaxLength(1000);
                permission.Property(x => x.DisplayName).HasMaxLength(200);
                permission.Property(x => x.Enabled).IsRequired();

                permission.HasIndex(x => x.Name).IsUnique();

                permission.HasMany(x => x.PermissionRoles);
            });

            modelBuilder.Entity<RoleEntity>(role =>
            {
                role.ToTable("role");
                role.HasKey(x => x.Id);

                role.Property(x => x.Name).HasMaxLength(200).IsRequired();
                role.Property(x => x.Description).HasMaxLength(1000);
                role.Property(x => x.DisplayName).HasMaxLength(200);
                role.Property(x => x.Enabled).IsRequired();

                role.HasIndex(x => x.Name).IsUnique();

                role.HasMany(x => x.RoleSubjects);
            });

            modelBuilder.Entity<PolicyEntity>(policy =>
            {
                policy.ToTable("policy");
                policy.HasKey(x => x.Id);

                policy.Property(x => x.Name).HasMaxLength(200).IsRequired();
                policy.Property(x => x.Description).HasMaxLength(1000);
                policy.Property(x => x.DisplayName).HasMaxLength(200);
                policy.Property(x => x.Enabled).IsRequired();

                policy.HasIndex(x => x.Name).IsUnique();

                policy.HasMany(x => x.PolicyRoles);
                policy.HasMany(x => x.PolicyPermissions);
            });

            modelBuilder.Entity<RoleSubjectEntity>(roleSubject => 
            {
                roleSubject.ToTable("role_subject");
                roleSubject.HasKey(x => new {x.RoleId, x.Subject});
            });
            modelBuilder.Entity<PermissionRoleEntity>(permissionRole => 
            {
                permissionRole.ToTable("permission_role");
                permissionRole.HasKey(x => new {x.PermissionId, x.RoleId});
            });
            modelBuilder.Entity<PolicyRoleEntity>(policyRole => 
            {
                policyRole.ToTable("policy_role");
                policyRole.HasKey(x => new {x.PolicyId, x.RoleId }); 
            });
            modelBuilder.Entity<PolicyPermissionEntity>(policyPermission => 
            {
                policyPermission.ToTable("policy_permission");
                policyPermission.HasKey(x => new {x.PolicyId, x.PermissionId }); 
            });
        }
    }
}
