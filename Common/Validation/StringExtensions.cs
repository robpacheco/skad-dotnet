using System;

namespace Skad.Common.Validation
{
    public static class StringExtensions
    {
        public static string RequireNonEmpty(this string value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(paramName);
            }

            return value;
        }
    }
}