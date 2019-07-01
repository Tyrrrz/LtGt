using System;

namespace LtGt.Internal.Selectors.Simple.StringOperators
{
    internal class StartsWithStringMatchOperator : StringMatchOperator
    {
        public override bool Matches(string haystack, string needle) => haystack.StartsWith(needle, StringComparison.Ordinal);

        public override string ToString() => "^";
    }
}