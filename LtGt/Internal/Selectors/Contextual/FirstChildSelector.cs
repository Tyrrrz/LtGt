using LtGt.Models;

namespace LtGt.Internal.Selectors.Contextual
{
    internal class FirstChildSelector : Selector
    {
        public Selector TargetSelector { get; }

        public FirstChildSelector(Selector targetSelector)
        {
            TargetSelector = targetSelector;
        }

        public override bool Matches(HtmlElement element) =>
            TargetSelector.Matches(element) && element.Previous == null;

        public override string ToString() => ":first-child";
    }
}