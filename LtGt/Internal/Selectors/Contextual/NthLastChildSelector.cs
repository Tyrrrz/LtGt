using LtGt.Models;

namespace LtGt.Internal.Selectors.Contextual
{
    internal class NthLastChildSelector : Selector
    {
        public Selector TargetSelector { get; }

        public int ChildIndex { get; }

        public NthLastChildSelector(Selector targetSelector, int childIndex)
        {
            TargetSelector = targetSelector;
            ChildIndex = childIndex;
        }

        public override bool Matches(HtmlElement element) =>
            element.Index == element.Parent.Children.Count - ChildIndex && TargetSelector.Matches(element);

        public override string ToString() => $"{TargetSelector}:nth-last-child({ChildIndex})";
    }
}