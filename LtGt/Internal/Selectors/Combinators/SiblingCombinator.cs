using LtGt.Models;

namespace LtGt.Internal.Selectors.Combinators
{
    internal class SiblingCombinator : Selector
    {
        public Selector PreviousSelector { get; }

        public Selector TargetSelector { get; }

        public SiblingCombinator(Selector previousSelector, Selector targetSelector)
        {
            PreviousSelector = previousSelector;
            TargetSelector = targetSelector;
        }

        public override bool Matches(HtmlElement element)
        {
            if (element.Previous is HtmlElement previousElement)
                return PreviousSelector.Matches(previousElement) && TargetSelector.Matches(element);

            return false;
        }

        public override string ToString() => $"{PreviousSelector} + {TargetSelector}";
    }
}