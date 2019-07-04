using System;
using System.Linq;
using LtGt.Models;

namespace LtGt.Internal.Selectors
{
    internal class LastOfTypeSelector : Selector
    {
        public override bool Matches(HtmlElement element) =>
            !element
                .GetNextSiblings()
                .OfType<HtmlElement>()
                .Any(e => string.Equals(e.Name, element.Name, StringComparison.OrdinalIgnoreCase));

        public override string ToString() => ":last-of-type";
    }
}