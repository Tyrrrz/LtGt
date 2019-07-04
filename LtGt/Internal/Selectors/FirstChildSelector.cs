using LtGt.Models;

namespace LtGt.Internal.Selectors
{
    internal class FirstChildSelector : Selector
    {
        public override bool Matches(HtmlElement element) => element.Previous == null;

        public override string ToString() => ":first-child";
    }
}