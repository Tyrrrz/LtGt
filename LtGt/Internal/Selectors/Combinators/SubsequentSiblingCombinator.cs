using System.Linq;
using LtGt.Models;

namespace LtGt.Internal.Selectors.Combinators
{
    internal class SubsequentSiblingCombinator : Selector
    {
        public Selector PreviousSelector { get; }

        public Selector TargetSelector { get; }

        public SubsequentSiblingCombinator(Selector previousSelector, Selector targetSelector)
        {
            PreviousSelector = previousSelector;
            TargetSelector = targetSelector;
        }

        public override bool Matches(HtmlElement element) =>
            TargetSelector.Matches(element) && element.GetPreviousSiblings().OfType<HtmlElement>().Any(PreviousSelector.Matches);

        public override string ToString() => "~";
    }
}