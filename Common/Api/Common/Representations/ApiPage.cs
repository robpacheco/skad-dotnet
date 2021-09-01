using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.Json.Serialization;
using Skad.Common.Validation;

namespace Skad.Common.Api.Common.Representations
{
    public class ApiPage<TItem>
    {
        public ApiPage(IList<TItem> items)
        {
            TotalCount = 0;
            Items = items.RequireNonEmptyParam(nameof(items)).ToImmutableList();
            Metadata = null;
        }

        public int TotalCount { get; }
        
        public IList<TItem> Items { get; }
        
        [JsonPropertyName("_meta")]
        public ApiMetadata? Metadata { get; }
    }
}