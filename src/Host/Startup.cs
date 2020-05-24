// Copyright (c) Brock Allen, Dominick Baier, Michele Leroux Bustamante. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Host.AspNetCorePolicy;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Host
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // `AddControllersWithViews()` Calls AddAuthorization under the hood.
            // Below, in the call to `UseEndpoints(...)` we require authorization to all controllers (besides controllers/actions that have `[AllowAnonymous]`).
            services.AddControllersWithViews();

            services.AddAuthentication("Cookies")
                .AddCookie("Cookies");

            //services
            //    .AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            //    .AddIdentityServerAuthentication(opt =>
            //        Configuration.GetSection("IdentityServerAuthenticationOptions")
            //            .Get<IdentityServerAuthenticationOptions>());

            // this sets up the PolicyServer client library and policy provider - configuration is loaded from appsettings.json
            services.AddPolicyServerClient(x =>
                {
                    x.IdentityServerEndpoint = "http://localhost:5001";
                    x.ClientId = "policyserver.client";
                    x.ClientSecret = "secret";
                    x.PolicyServerEndpoint = "http://localhost:5500";
                    x.PolicyName = "Hospital";
                    x.PolicySecret = "secret";
                    x.PolicyServerApiName = "policyserver.api";
                })
                .AddAuthorizationPermissionPolicies();

            // this adds the necessary handler for our custom medication requirement
            services.AddTransient<IAuthorizationHandler, MedicationRequirementHandler>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseRouting();

            // add this middleware to make roles and permissions available as claims
            // this is mainly useful for using the classic [Authorize(Roles="foo")] and IsInRole functionality
            // this is not needed if you use the client library directly or the new policy-based authorization framework in ASP.NET Core
            app.UsePolicyServerClaims();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute().RequireAuthorization();
            });
        }
    }
}