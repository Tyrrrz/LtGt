using LtGt.Models;

namespace LtGt.Internal.Selectors.Combinators
{
    internal class ChildCombinator : Selector
    {
        public Selector ParentSelector { get; }

        public Selector TargetSelector { get; }

        public ChildCombinator(Selector parentSelector, Selector targetSelector)
        {
            ParentSelector = parentSelector;
            TargetSelector = targetSelector;
        }

        public override bool Matches(HtmlElement element) =>
            element.Parent is HtmlElement parentElement && ParentSelector.Matches(parentElement) &&
            TargetSelector.Matches(element);

        public override string ToString() => $"{ParentSelector} > {TargetSelector}";
    }
}