using System;

namespace LtGt.Internal.Selectors.Simple.StringOperators
{
    internal class EndsWithStringMatchOperator : StringMatchOperator
    {
        public override bool Matches(string haystack, string needle) => haystack.EndsWith(needle, StringComparison.Ordinal);

        public override string ToString() => "$";
    }
}