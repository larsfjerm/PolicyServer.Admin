using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PolicyServer.EntityFramework.Extensions;

namespace PolicyServer.Host
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
            services.AddPolicyServerEntityFramework(Configuration.GetConnectionString("PolicyServer.EF.ConenctionString"), typeof(Startup).Assembly.FullName);

            services
                .AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(opt =>
                {
                    opt.Authority = "http://localhost:5000";
                    opt.ApiName = "policyserver.api";
                    opt.ApiSecret = "secret";
                    opt.RequireHttpsMetadata = false;
                });
                    //Configuration.GetSection("IdentityServerAuthenticationOptions")
                    //.Get<IdentityServerAuthenticationOptions>());

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UsePolicyServerEntityFramework();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
