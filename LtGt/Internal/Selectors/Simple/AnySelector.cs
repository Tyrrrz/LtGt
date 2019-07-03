using LtGt.Models;

namespace LtGt.Internal.Selectors.Simple
{
    internal class AnySelector : Selector
    {
        public override bool Matches(HtmlElement element) => true;

        public override string ToString() => "*";
    }
}