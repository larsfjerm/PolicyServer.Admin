using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PolicyServer.EntityFramework.DbContexts;
using PolicyServer.EntityFramework.Entities;

namespace PolicyServer.EntityFramework.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPolicyServerEntityFramework(this IServiceCollection services,
            string connectionString, string migrationsAssembly)
        {
            services
                .AddEntityFrameworkNpgsql()
                .AddDbContext<ConfigurationDbContext>(options =>
                    options.UseNpgsql(connectionString, b => b.MigrationsAssembly(migrationsAssembly)));
            return services;
        }
    }

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UsePolicyServerEntityFramework(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

                context.Database.Migrate();

                if (context.Policies.Any(x => x.Name == "Default"))
                    return app;

                var role = new Role
                {
                    Name = "Test Role",
                    RoleSubjects = new List<RoleSubject>
                    {
                        new RoleSubject {Subject = "a-sub"}
                    }
                };

                context.Policies.Add(new Policy
                {
                    Name = "Default",
                    PolicyRoles = new List<PolicyRole>
                    {
                        new PolicyRole
                        {
                            Role = role
                        }
                    },
                    PolicyPermissions = new List<PolicyPermission>
                    {
                        new PolicyPermission
                        {
                            Permission = new Permission
                            {
                                Name = "Admin_Permission",
                                PermissionRoles = new List<PermissionRole>
                                {
                                    new PermissionRole
                                    {
                                        Role = role
                                    }
                                }
                            }
                        }
                    }
                });

                context.SaveChanges();
            }

            return app;
        }
    }
}
