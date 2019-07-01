using System;

namespace LtGt.Internal.Selectors.Simple.StringOperators
{
    internal class ContainsStringMatchOperator : StringMatchOperator
    {
        public override bool Matches(string haystack, string needle) => haystack.IndexOf(needle, StringComparison.Ordinal) >= 0;

        public override string ToString() => "*";
    }
}