using System.Linq;
using LtGt.Models;

namespace LtGt.Internal.Selectors.Contextual
{
    internal class DescendantSelector : Selector
    {
        public Selector ParentSelector { get; }

        public Selector TargetSelector { get; }

        public DescendantSelector(Selector parentSelector, Selector targetSelector)
        {
            ParentSelector = parentSelector;
            TargetSelector = targetSelector;
        }

        public override bool Matches(HtmlElement element) =>
            TargetSelector.Matches(element) && element.GetAncestorElements().Any(ParentSelector.Matches);

        public override string ToString() => $"{ParentSelector} {TargetSelector}";
    }
}