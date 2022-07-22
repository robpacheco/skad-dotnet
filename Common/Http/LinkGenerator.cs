using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Skad.Common.Http
{
    public class LinkGenerator
    {
        private readonly EndpointSettings _endpointSettings;
        private readonly IHttpContextAccessor _contextAccessor;

        public LinkGenerator(IOptions<EndpointSettings> endpointSettings, IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
            _endpointSettings = endpointSettings.Value ?? throw new ArgumentNullException(nameof(endpointSettings));
        }

        public Uri GenerateVulnerabilityFeedUri(params string[] pathComponents)
        {
            var uriBuilder = MakeUriBuilder(_endpointSettings.VulnerabilityFeedBaseUrl);
            uriBuilder.Path = Path.Combine(_endpointSettings.VulnerabilityFeedPrefix ?? "", Path.Combine(pathComponents));
        
            return uriBuilder.Uri;
        }
    
        public Uri GenerateSubscriptionUri(params string[] pathComponents)
        {
            var uriBuilder = MakeUriBuilder(_endpointSettings.SubscriptionBaseUrl);
            uriBuilder.Path = Path.Combine(_endpointSettings.SubscriptionPrefix ?? "", Path.Combine(pathComponents));
        
            return uriBuilder.Uri;
        }

        private UriBuilder MakeUriBuilder(string? settingsBaseUrl)
        {
            var req = GetHttpRequest();
            var baseUrl = $"{req.Scheme}://{req.Host}";
            if (!string.IsNullOrWhiteSpace(settingsBaseUrl))
            {
                baseUrl = settingsBaseUrl;
            }

            return new UriBuilder(baseUrl);
        }

        private HttpRequest GetHttpRequest()
        {
            if (_contextAccessor.HttpContext == null)
            {
                throw new Exception("Cannot get HttpContext.");
            }

            return _contextAccessor.HttpContext.Request;
        }
    }
}