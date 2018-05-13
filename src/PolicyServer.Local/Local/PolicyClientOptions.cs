using System;

namespace PolicyServer.Local
{
    public class PolicyClientOptions
    {
        /// <summary>
        /// IdentityServer Uri
        /// </summary>
        public string IdentityServerEndpoint { get; set; }

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public string PolicyServerApiName { get; set; }

        /// <summary>
        /// PolicyServer Uri
        /// </summary>
        public string PolicyServerEndpoint { get; set; }

        public string PolicyName { get; set; } = "DefaultPolicy";
        public string PolicySecret { get; set; }

        public TimeSpan PolicyUpdaterInterval { get; set; } = TimeSpan.FromSeconds(30);
    }
}