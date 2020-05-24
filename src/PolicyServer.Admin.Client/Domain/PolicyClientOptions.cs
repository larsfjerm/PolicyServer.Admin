using System;

namespace PolicyServer.Admin.Client
{
    public class PolicyClientOptions
    {
        public string IdentityServerEndpoint { get; set; }

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public string PolicyServerApiName { get; set; }

        public string PolicyServerEndpoint { get; set; }

        public string PolicyName { get; set; }
        public string PolicySecret { get; set; }

        public TimeSpan PolicyUpdaterInterval { get; set; } = TimeSpan.FromSeconds(30);
    }
}