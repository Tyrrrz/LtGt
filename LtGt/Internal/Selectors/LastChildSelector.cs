using LtGt.Models;

namespace LtGt.Internal.Selectors
{
    internal class LastChildSelector : Selector
    {
        public override bool Matches(HtmlElement element) => element.Next == null;

        public override string ToString() => ":last-child";
    }
}