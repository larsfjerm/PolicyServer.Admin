using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PolicyServer.EntityFramework.DbContexts;
using PolicyServer.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicyServer.Admin
{
    public static class DbHelper
    {
        public static async Task EnsureSeedData(IHost host)
        {
            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                await EnsureDatabasesMigrated(services);
                await EnsureSeedData(services);
            }
        }

        private static async Task EnsureDatabasesMigrated(IServiceProvider services)
        {
            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>())
                {
                    await context.Database.MigrateAsync();
                }
            }
        }

        private static async Task EnsureSeedData(IServiceProvider services)
        {
            using var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

            if (context.Policies.Any(x => x.Name == "Default"))
                return;

            var role = new RoleEntity
            {
                Name = "Test Role",
                RoleSubjects = new List<RoleSubjectEntity>
                            {
                                new RoleSubjectEntity {Subject = "a-sub"}
                            }
            };

            context.Policies.Add(new PolicyEntity
            {
                Name = "Default",
                PolicyRoles = new List<PolicyRoleEntity>
                            {
                                new PolicyRoleEntity
                                {
                                    Role = role
                                }
                            },
                PolicyPermissions = new List<PolicyPermissionEntity>
                            {
                                new PolicyPermissionEntity
                                {
                                    Permission = new PermissionEntity
                                    {
                                        Name = "Admin_Permission",
                                        PermissionRoles = new List<PermissionRoleEntity>
                                        {
                                            new PermissionRoleEntity
                                            {
                                                Role = role
                                            }
                                        }
                                    }
                                }
                            }
            });

            await context.SaveChangesAsync();
        }

    }
}
