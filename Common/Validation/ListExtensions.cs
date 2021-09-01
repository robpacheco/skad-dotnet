using System;
using System.Collections.Generic;
using System.Linq;

namespace Skad.Common.Validation
{
    public static class ListExtensions
    {
        public static IList<TElement> RequireNonEmptyParam<TElement>(this IList<TElement> list, string paramName)
        {
            if (list == null || !list.Any())
            {
                throw new ArgumentNullException(paramName); // TODO: Maybe a different exception?
            }

            return list;
        }
    }
}