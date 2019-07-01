using System;
using System.Linq;

namespace LtGt.Internal.Selectors.Simple.StringOperators
{
    internal class HyphenSeparatedStartsWithStringMatchOperator : StringMatchOperator
    {
        public override bool Matches(string haystack, string needle) =>
            string.Equals(haystack.Split(new[] {'-'}, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(), needle,
                StringComparison.Ordinal);

        public override string ToString() => "|";
    }
}