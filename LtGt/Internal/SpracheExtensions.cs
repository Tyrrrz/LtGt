using Sprache;

namespace LtGt.Internal
{
    internal static class SpracheExtensions
    {
        public static Parser<T> TokenLeft<T>(this Parser<T> parser) => Parse.WhiteSpace.Many().Then(_ => parser);

        public static Parser<T> TokenRight<T>(this Parser<T> parser) => parser.Then(o => Parse.WhiteSpace.Many().Return(o));
    }
}