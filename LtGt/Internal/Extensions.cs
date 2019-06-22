using System.Collections.Generic;
using System.Text;

namespace LtGt.Internal
{
    internal static class Extensions
    {
        public static bool IsNullOrWhiteSpace(this string s) => string.IsNullOrWhiteSpace(s);

        public static string JoinToString<T>(this IEnumerable<T> values, string separator) => string.Join(separator, values);

        public static string Concatenate(this IEnumerable<string> values)
        {
            var buffer = new StringBuilder();

            foreach (var value in values)
                buffer.Append(value);

            return buffer.ToString();
        }
    }
}