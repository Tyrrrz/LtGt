using LtGt.Models;

namespace LtGt.Internal.Selectors
{
    internal class RootSelector : Selector
    {
        public override bool Matches(HtmlElement element) => element.Parent == null || element.Parent is HtmlDocument;

        public override string ToString() => ":root";
    }
}