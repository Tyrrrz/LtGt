using LtGt.Models;

namespace LtGt.Internal.Selectors.Contextual
{
    internal class LastChildSelector : Selector
    {
        public Selector TargetSelector { get; }

        public LastChildSelector(Selector targetSelector)
        {
            TargetSelector = targetSelector;
        }

        public override bool Matches(HtmlElement element) =>
            TargetSelector.Matches(element) && element.Next == null;

        public override string ToString() => ":last-child";
    }
}