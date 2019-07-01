using System.Collections.Generic;
using System.Linq;

namespace LtGt.Internal
{
    internal static class Extensions
    {
        public static bool IsNullOrWhiteSpace(this string s) => string.IsNullOrWhiteSpace(s);

        public static bool AllEquals<T>(this IEqualityComparer<T> comparer, IEnumerable<T> x, IEnumerable<T> y) =>
            x.SequenceEqual(y, comparer);
    }
}