using LtGt.Models;

namespace LtGt.Internal.Selectors.Contextual
{
    internal class ChildSelector : Selector
    {
        public Selector ParentSelector { get; }

        public Selector TargetSelector { get; }

        public ChildSelector(Selector parentSelector, Selector targetSelector)
        {
            ParentSelector = parentSelector;
            TargetSelector = targetSelector;
        }

        public override bool Matches(HtmlElement element) =>
            TargetSelector.Matches(element) &&
            element.Parent is HtmlElement parentElement && ParentSelector.Matches(parentElement);

        public override string ToString() => $"{ParentSelector} > {TargetSelector}";
    }
}