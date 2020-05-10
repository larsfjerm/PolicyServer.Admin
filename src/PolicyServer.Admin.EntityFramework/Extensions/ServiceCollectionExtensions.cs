using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PolicyServer.EntityFramework.DbContexts;

namespace PolicyServer.EntityFramework.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPolicyServerEntityFramework(this IServiceCollection services,
            string connectionString, string migrationsAssembly)
        {
            services
                .AddDbContext<ConfigurationDbContext>(options =>
                    options.UseNpgsql(connectionString, b => b.MigrationsAssembly(migrationsAssembly)));
            return services;
        }
    }
}
