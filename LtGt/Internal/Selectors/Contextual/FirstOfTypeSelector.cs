using System;
using System.Linq;
using LtGt.Models;

namespace LtGt.Internal.Selectors.Contextual
{
    internal class FirstOfTypeSelector : Selector
    {
        public Selector TargetSelector { get; }

        public FirstOfTypeSelector(Selector targetSelector)
        {
            TargetSelector = targetSelector;
        }

        public override bool Matches(HtmlElement element) =>
            TargetSelector.Matches(element) && !element.GetPreviousSiblings().OfType<HtmlElement>()
                .Any(e => string.Equals(e.Name, element.Name, StringComparison.OrdinalIgnoreCase));

        public override string ToString() => ":first-of-type";
    }
}