using System;
using System.Linq;

namespace LtGt.Internal.Selectors.Simple.StringOperators
{
    internal class WhiteSpaceSeparatedContainsStringMatchOperator : StringMatchOperator
    {
        public override bool Matches(string haystack, string needle) =>
            haystack.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).Contains(needle, StringComparer.Ordinal);

        public override string ToString() => "~";
    }
}