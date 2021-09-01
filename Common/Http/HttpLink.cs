using Skad.Common.Validation;

namespace Skad.Common.Http
{
    public class HttpLink
    {
        public HttpLink(string rel, string location)
        {
            Rel = rel.RequireNonEmpty(nameof(rel));
            Location = location.RequireNonEmpty(nameof(location));
        }

        public string Rel { get; }
        
        public string Location { get; }
    }
}