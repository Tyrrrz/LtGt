using LtGt.Models;

namespace LtGt.Internal.Selectors.Contextual
{
    internal class SiblingSelector : Selector
    {
        public Selector PreviousSelector { get; }

        public Selector TargetSelector { get; }

        public SiblingSelector(Selector previousSelector, Selector targetSelector)
        {
            PreviousSelector = previousSelector;
            TargetSelector = targetSelector;
        }

        public override bool Matches(HtmlElement element) =>
            TargetSelector.Matches(element) &&
            element.Previous is HtmlElement previousElement && PreviousSelector.Matches(previousElement);

        public override string ToString() => $"{PreviousSelector} + {TargetSelector}";
    }
}