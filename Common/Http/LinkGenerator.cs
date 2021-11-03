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

        public LinkGenerator(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        public Uri GenerateUri(params string[] pathComponents)
        {
            var httpRequest = _contextAccessor.HttpContext.Request;
            var uriBuilder = new UriBuilder {Scheme = httpRequest.Scheme, Host = httpRequest.Host.Host};
            var port = httpRequest.Host.Port;

            if (port.HasValue)
            {
                uriBuilder.Port = port.Value;
            }

            uriBuilder.Path = Path.Combine(pathComponents);
            return uriBuilder.Uri;
        }
    }
}