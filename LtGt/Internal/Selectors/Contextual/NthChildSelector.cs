using LtGt.Models;

namespace LtGt.Internal.Selectors.Contextual
{
    internal class NthChildSelector : Selector
    {
        public Selector TargetSelector { get; }

        public int ChildIndex { get; }

        public NthChildSelector(Selector targetSelector, int childIndex)
        {
            TargetSelector = targetSelector;
            ChildIndex = childIndex;
        }

        public override bool Matches(HtmlElement element) =>
            element.Index + 1 == ChildIndex && TargetSelector.Matches(element);

        public override string ToString() => $"{TargetSelector}:nth-child({ChildIndex})";
    }
}