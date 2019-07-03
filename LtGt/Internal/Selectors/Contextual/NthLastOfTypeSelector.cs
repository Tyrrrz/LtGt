using System;
using System.Linq;
using LtGt.Models;

namespace LtGt.Internal.Selectors.Contextual
{
    internal class NthLastOfTypeSelector : Selector
    {
        public Selector TargetSelector { get; }

        public int ChildIndex { get; }

        public NthLastOfTypeSelector(Selector targetSelector, int childIndex)
        {
            TargetSelector = targetSelector;
            ChildIndex = childIndex;
        }

        public override bool Matches(HtmlElement element) =>
            TargetSelector.Matches(element) &&
            element.GetNextSiblings().OfType<HtmlElement>().Count(e => string.Equals(e.Name, element.Name, StringComparison.OrdinalIgnoreCase)) == ChildIndex - 1;

        public override string ToString() => $"{TargetSelector}:nth-last-of-type({ChildIndex})";
    }
}