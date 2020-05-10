// Copyright (c) Brock Allen, Dominick Baier, Michele Leroux Bustamante. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PolicyServer.Admin.Client;
using PolicyServer.Runtime.Client;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Helper class to configure DI
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the policy server client.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="policyClientOptions">Option settings</param>
        /// <returns></returns>
        public static PolicyServerBuilder AddPolicyServerClient(this IServiceCollection services,
            Action<PolicyClientOptions> policyClientOptions)
        {
            var options = new PolicyClientOptions();
            policyClientOptions.Invoke(options);

            if(string.IsNullOrWhiteSpace(options.IdentityServerEndpoint)) throw new InvalidOperationException("IdentityServerEndpoint is not configured");
            if(string.IsNullOrWhiteSpace(options.ClientId)) throw new InvalidOperationException("ClientId is not configured");
            if(string.IsNullOrWhiteSpace(options.PolicyServerEndpoint)) throw new InvalidOperationException("PolicyServerEndpoint is not configured");
            if(string.IsNullOrWhiteSpace(options.PolicyName)) throw new InvalidOperationException("PolicyName is not configured");
            if(string.IsNullOrWhiteSpace(options.PolicySecret)) throw new InvalidOperationException("PolicySecret is not configured");
            if(string.IsNullOrWhiteSpace(options.PolicyServerApiName)) throw new InvalidOperationException("PolicyServerApiName is not configured");

            services.Configure(policyClientOptions);

            var policy = GetPolicy(options).Result;
            services.AddSingleton(policy);

            var policyAction = new Action<Policy>(x =>
            {
                x.Roles = policy.Roles;
                x.Permissions = policy.Permissions;
            });

            services.Configure(policyAction);

            services.AddTransient<IPolicyServerRuntimeClient, PolicyServerRuntimeClient>();
            services.AddScoped(provider => provider.GetRequiredService<IOptionsSnapshot<Policy>>().Value);
            services.AddScoped(provider => provider.GetRequiredService<IOptionsSnapshot<PolicyClientOptions>>().Value);

            return new PolicyServerBuilder(services);
        }

        private static async Task<Policy> GetPolicy(PolicyClientOptions options)
        {
            var client = new HttpClient();

            // discover endpoints from metadata
            var disco = await client.GetDiscoveryDocumentAsync(options.IdentityServerEndpoint);
            if (disco.IsError)
            {
                throw new InvalidCredentialException("Invalid - " + disco.Error);
            }

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = options.ClientId,
                ClientSecret = options.ClientSecret,
                Scope = options.PolicyServerApiName, 
            });

            if (tokenResponse.IsError)
            {
                throw new InvalidCredentialException("Invalid token - " + tokenResponse.Error);
            }

            // call policy server
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync($"{options.PolicyServerEndpoint}/policies/{options.PolicyName}?secret={options.PolicySecret}");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Unsuccessful request - " + response.StatusCode);
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Policy>(content);
        }
    }
}