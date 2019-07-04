using System;
using System.Linq;
using LtGt.Models;

namespace LtGt.Internal.Selectors
{
    internal class LastOfTypeSelector : Selector
    {
        public override bool Matches(HtmlElement element)
        {
            var nextSiblingsOfSameType = element.GetNextSiblings()
                .OfType<HtmlElement>()
                .Where(e => string.Equals(e.Name, element.Name, StringComparison.OrdinalIgnoreCase))
                .ToArray();

            return !nextSiblingsOfSameType.Any();
        }

        public override string ToString() => ":last-of-type";
    }
}