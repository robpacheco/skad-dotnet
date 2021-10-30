using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Skad.Common.Api.Common.Representations;

namespace Skad.Common.Http
{
    public class LinkGenerator
    {
        private readonly EndpointSettings _endpointSettings;

        public LinkGenerator(IOptions<EndpointSettings> endpointSettings)
        {
            _endpointSettings = endpointSettings.Value ?? throw new ArgumentNullException(nameof(endpointSettings));
        }

        public Uri GenerateVulnerabilityFeedUri(params string[] pathComponents)
        {
            if (string.IsNullOrWhiteSpace(_endpointSettings.VulnerabilityFeedBaseUrl))
            {
                throw new Exception("VulnerabilityFeedBaseUrl must have a value.");
            }
            
            var uriBuilder = MakeUriBuilder(_endpointSettings.VulnerabilityFeedBaseUrl);
            uriBuilder.Path = Path.Combine(pathComponents);
            return uriBuilder.Uri;
        }

        public Uri GenerateSubscriptionUri(params string[] pathComponents)
        {
            if (string.IsNullOrWhiteSpace(_endpointSettings.SubscriptionBaseUrl))
            {
                throw new Exception("SubscriptionBaseUrl must have a value.");
            }
            
            var uriBuilder = MakeUriBuilder(_endpointSettings.SubscriptionBaseUrl);
            uriBuilder.Path = Path.Combine(pathComponents);
            return uriBuilder.Uri;
        }

        private UriBuilder MakeUriBuilder(string baseUrl)
        {
            var uriBuilder = new UriBuilder(baseUrl);
            return uriBuilder;
        }
    }
}