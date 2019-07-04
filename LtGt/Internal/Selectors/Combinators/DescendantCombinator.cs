using System.Linq;
using LtGt.Models;

namespace LtGt.Internal.Selectors.Combinators
{
    internal class DescendantCombinator : Selector
    {
        public Selector AncestorSelector { get; }

        public Selector TargetSelector { get; }

        public DescendantCombinator(Selector ancestorSelector, Selector targetSelector)
        {
            AncestorSelector = ancestorSelector;
            TargetSelector = targetSelector;
        }

        public override bool Matches(HtmlElement element) =>
            element.GetAncestors().OfType<HtmlElement>().Any(AncestorSelector.Matches) &&
            TargetSelector.Matches(element);

        public override string ToString() => $"{AncestorSelector} {TargetSelector}";
    }
}