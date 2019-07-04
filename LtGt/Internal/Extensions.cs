using Sprache;

namespace LtGt.Internal
{
    internal static class Extensions
    {
        public static bool IsNullOrWhiteSpace(this string s) => string.IsNullOrWhiteSpace(s);

        public static Parser<T> TokenLeft<T>(this Parser<T> parser) => i =>
        {
            while (!i.AtEnd && char.IsWhiteSpace(i.Current))
                i = i.Advance();

            return parser(i);
        };
    }
}