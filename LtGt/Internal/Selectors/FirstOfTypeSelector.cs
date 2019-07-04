using System;
using System.Linq;
using LtGt.Models;

namespace LtGt.Internal.Selectors
{
    internal class FirstOfTypeSelector : Selector
    {
        public override bool Matches(HtmlElement element) =>
            !element
                .GetPreviousSiblings()
                .OfType<HtmlElement>()
                .Any(e => string.Equals(e.Name, element.Name, StringComparison.OrdinalIgnoreCase));

        public override string ToString() => ":first-of-type";
    }
}