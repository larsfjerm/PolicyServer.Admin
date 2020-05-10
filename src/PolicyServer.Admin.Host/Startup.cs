using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();

            // add this middleware to make roles and permissions available as claims
            // this is mainly useful for using the classic [Authorize(Roles="foo")] and IsInRole functionality
            // this is not needed if you use the client library directly or the new policy-based authorization framework in ASP.NET Core
            app.UsePolicyServerClaims();
            //app.UsePolicyServerEntityFramework();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute().RequireAuthorization();
            });
        }
    }
}
