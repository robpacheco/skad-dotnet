using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Skad.Common.Api.Common.Representations;

namespace Skad.Common.Http
{
    public class LinkGenerator
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpSettings _httpSettings;

        public LinkGenerator(IHttpContextAccessor contextAccessor, IOptions<HttpSettings> apiSettings)
        {
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
            _httpSettings = apiSettings.Value ?? throw new ArgumentNullException(nameof(apiSettings));
        }

        public HttpLink GenerateLink(string rel, params string[] pathComponents)
        {
            var url = GenerateUri(pathComponents).ToString();
            return new HttpLink(rel, url);
        }

        public Uri GenerateUri(params string[] pathComponents)
        {
            var httpRequest = GetHttpRequest();
            var uriBuilder = new UriBuilder {Scheme = httpRequest.Scheme, Host = httpRequest.Host.Host};
            var port = _httpSettings.HttpPort ?? httpRequest.Host.Port; // TODO: What does port look like in https://example.com/api/...

            if (port.HasValue)
            {
                uriBuilder.Port = port.Value;
            }

            uriBuilder.Path = Path.Combine(pathComponents);
            return uriBuilder.Uri;
        }

        private HttpRequest GetHttpRequest()
        {
            // TODO: Check request too...
            return _contextAccessor.HttpContext.Request;
        }
    }
}