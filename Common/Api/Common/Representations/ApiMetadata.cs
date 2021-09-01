using System.Collections.Generic;
using System.Collections.Immutable;
using Skad.Common.Http;
using Skad.Common.Validation;

namespace Skad.Common.Api.Common.Representations
{
    public class ApiMetadata
    {
        public ApiMetadata(string location)
        {
            Location = location.RequireNonEmpty(nameof(location));
            Links = ImmutableList<HttpLink>.Empty;
        }

        public ApiMetadata(string location, IList<HttpLink> links)
        {
            Location = location.RequireNonEmpty(nameof(location));
            Links = links.RequireNonEmptyParam(nameof(links)).ToImmutableList();
        }

        public string Location { get; }
        
        public IList<HttpLink> Links { get; }
    }
}