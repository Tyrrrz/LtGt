using System.Collections.Generic;

namespace LtGt.Internal
{
    internal static class Extensions
    {
        public static bool IsNullOrWhiteSpace(this string s) => string.IsNullOrWhiteSpace(s);

        public static IEnumerable<T> ToEnumerable<T>(this T value)
        {
            if (value != null)
                yield return value;
        }
    }
}