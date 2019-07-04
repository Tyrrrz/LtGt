using System;
using System.Linq;
using LtGt.Models;

namespace LtGt.Internal.Selectors
{
    internal class FirstOfTypeSelector : Selector
    {
        public override bool Matches(HtmlElement element)
        {
            var previousSiblingsOfSameType = element.GetPreviousSiblings()
                .OfType<HtmlElement>()
                .Where(e => string.Equals(e.Name, element.Name, StringComparison.OrdinalIgnoreCase))
                .ToArray();

            return !previousSiblingsOfSameType.Any();
        }

        public override string ToString() => ":first-of-type";
    }
}