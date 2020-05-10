using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PolicyServer.EntityFramework.DbContexts;
using PolicyServer.EntityFramework.Extensions;
using System.Threading.Tasks;

namespace PolicyServer.Admin
{
    internal static class Program
    {
        public static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                    .ConfigureServices((host, services) =>
                    {
                        services.AddPolicyServerEntityFramework(host.Configuration.GetConnectionString("PolicyServer.EF.ConenctionString"), typeof(ConfigurationDbContext).Assembly.GetName().Name);



                        //services.AddMvc(x => x.)
                    })
                    .ConfigureLogging((hostingContext, logging) =>
                    {
                        logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                        logging.AddConsole();
                    })
                    .ConfigureWebHostDefaults(webBuilder => 
                    {
                        webBuilder.UseStartup<Startup>();

                        //webBuilder.Configure(app =>
                        //{
                        //    app.UseRouting();

                        //    app.UseAuthorization();

                        //    app.UseEndpoints(endpoints =>
                        //    {
                        //        //endpoints.MapDefaultControllerRoute().RequireAuthorization();
                        //        endpoints.MapControllers();
                        //    });
                        //});
                    })
                    .Build();

            await DbHelper.EnsureSeedData(host);

            host.Run();
        }
    }
}
