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

            if (context.Policies.Any(x => x.Name == "Hospital"))
                return;

            var doctorRole = new RoleEntity
            {
                Name = "doctor",
                RoleSubjects = new List<RoleSubjectEntity>
                {
                    new RoleSubjectEntity {Subject = "1" }
                }
            };
            var nurseRole = new RoleEntity
            {
                Name = "nurse",
                RoleSubjects = new List<RoleSubjectEntity>
                {
                    new RoleSubjectEntity {Subject = "11" }
                }
            };
            var patientRole = new RoleEntity
            {
                Name = "patient",
                RoleSubjects = new List<RoleSubjectEntity>
                {
                    new RoleSubjectEntity {Subject = "99" }
                }
            };

            context.Policies.Add(new PolicyEntity
            {
                Name = "Hospital",
                PolicyRoles = new List<PolicyRoleEntity>
                {
                    new PolicyRoleEntity { Role = doctorRole },
                    new PolicyRoleEntity { Role = nurseRole },
                    new PolicyRoleEntity { Role = patientRole }
                },
                PolicyPermissions = new List<PolicyPermissionEntity>
                {
                    new PolicyPermissionEntity
                    {
                        Permission = new PermissionEntity
                        {
                            Name = "SeePatients",
                            PermissionRoles = new List<PermissionRoleEntity>
                            {
                                new PermissionRoleEntity { Role = doctorRole },
                                new PermissionRoleEntity { Role = nurseRole }
                            }
                        }
                    },
                    new PolicyPermissionEntity
                    {
                        Permission = new PermissionEntity
                        {
                            Name = "PerformSurgery",
                            PermissionRoles = new List<PermissionRoleEntity>
                            {
                                new PermissionRoleEntity { Role = doctorRole }
                            }
                        }
                    },
                    new PolicyPermissionEntity
                    {
                        Permission = new PermissionEntity
                        {
                            Name = "PrescribeMedication",
                            PermissionRoles = new List<PermissionRoleEntity>
                            {
                                new PermissionRoleEntity { Role = doctorRole },
                                new PermissionRoleEntity { Role = nurseRole }
                            }
                        }
                    },
                    new PolicyPermissionEntity
                    {
                        Permission = new PermissionEntity
                        {
                            Name = "RequestPainMedication",
                            PermissionRoles = new List<PermissionRoleEntity>
                            {
                                new PermissionRoleEntity { Role = patientRole },
                            }
                        }
                    }
                }
            });

            await context.SaveChangesAsync();
        }

    }
}
