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

        public override bool Matches(HtmlElement element)
        {
            if (element.Parent is HtmlElement parentElement)
                return ParentSelector.Matches(parentElement) && TargetSelector.Matches(element);

            return false;
        }

        public override string ToString() => $"{ParentSelector} > {TargetSelector}";
    }
}