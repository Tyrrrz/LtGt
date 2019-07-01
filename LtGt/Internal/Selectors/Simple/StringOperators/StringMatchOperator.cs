namespace LtGt.Internal.Selectors.Simple.StringOperators
{
    internal abstract class StringMatchOperator
    {
        public abstract bool Matches(string haystack, string needle);
    }
}