using System;
using System.Linq;
using LtGt.Models;

namespace LtGt.Internal.Selectors.Contextual
{
    internal class OnlyOfTypeSelector : Selector
    {
        public Selector TargetSelector { get; }

        public OnlyOfTypeSelector(Selector targetSelector)
        {
            TargetSelector = targetSelector;
        }

        public override bool Matches(HtmlElement element) =>
            TargetSelector.Matches(element) && !element.GetSiblings().OfType<HtmlElement>()
                .Any(e => string.Equals(e.Name, element.Name, StringComparison.OrdinalIgnoreCase));

        public override string ToString() => ":only-of-type";
    }
}