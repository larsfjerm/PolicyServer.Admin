using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PolicyServer.EntityFramework.DbContexts;
using PolicyServer.EntityFramework.Extensions;

namespace PolicyServer.Admin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPolicyServerEntityFramework(Configuration.GetConnectionString("PolicyServer.EF.ConenctionString"), typeof(ConfigurationDbContext).Assembly.GetName().Name);

            services
                .AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(opt =>
                {
                    opt.Authority = "http://localhost:5001";
                    opt.ApiName = "policyserver.api";
                    opt.ApiSecret = "secret";
                    opt.RequireHttpsMetadata = false;
                });
            //Configuration.GetSection("IdentityServerAuthenticationOptions")

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
            });
        }
    }
}
