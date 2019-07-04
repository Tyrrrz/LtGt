using System;
using System.Linq;

namespace LtGt.Internal.Selectors.Terms
{
    internal class StringComparisonTerm
    {
        public StringComparisonStrategy Strategy { get; }

        public StringComparisonTerm(StringComparisonStrategy strategy = StringComparisonStrategy.Equals)
        {
            Strategy = strategy;
        }

        public bool Matches(string source, string value)
        {
            if (source is null || value is null)
                return false;

            if (Strategy == StringComparisonStrategy.Equals)
            {
                return string.Equals(source, value, StringComparison.Ordinal);
            }

            if (Strategy == StringComparisonStrategy.ContainsWithinWhiteSpaceSeparated)
            {
                var separated = source.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                return separated.Contains(value, StringComparer.Ordinal);
            }

            if (Strategy == StringComparisonStrategy.StartsWith)
            {
                return source.StartsWith(value, StringComparison.Ordinal);
            }

            if (Strategy == StringComparisonStrategy.EndsWith)
            {
                return source.EndsWith(value, StringComparison.Ordinal);
            }

            if (Strategy == StringComparisonStrategy.Contains)
            {
                return source.IndexOf(value, StringComparison.Ordinal) >= 0;
            }

            if (Strategy == StringComparisonStrategy.StartsWithHyphenSeparated)
            {
                var separated = source.Split(new[] {'-'}, StringSplitOptions.RemoveEmptyEntries);
                return string.Equals(separated.FirstOrDefault(), value, StringComparison.Ordinal);
            }

            throw new InvalidOperationException($"Unknown strategy [{Strategy}].");
        }

        public override string ToString()
        {
            if (Strategy == StringComparisonStrategy.Equals)
                return "";

            if (Strategy == StringComparisonStrategy.ContainsWithinWhiteSpaceSeparated)
                return "~";

            if (Strategy == StringComparisonStrategy.StartsWith)
                return "^";

            if (Strategy == StringComparisonStrategy.EndsWith)
                return "$";

            if (Strategy == StringComparisonStrategy.Contains)
                return "*";

            if (Strategy == StringComparisonStrategy.StartsWithHyphenSeparated)
                return "|";

            throw new InvalidOperationException($"Unknown strategy [{Strategy}].");
        }
    }
}