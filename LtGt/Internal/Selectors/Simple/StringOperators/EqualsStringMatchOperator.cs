using System;

namespace LtGt.Internal.Selectors.Simple.StringOperators
{
    internal class EqualsStringMatchOperator : StringMatchOperator
    {
        public override bool Matches(string haystack, string needle) => string.Equals(haystack, needle, StringComparison.Ordinal);

        public override string ToString() => "";
    }
}