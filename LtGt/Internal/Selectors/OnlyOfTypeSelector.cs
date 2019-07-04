using System;
using System.Linq;
using LtGt.Models;

namespace LtGt.Internal.Selectors
{
    internal class OnlyOfTypeSelector : Selector
    {
        public override bool Matches(HtmlElement element) =>
            !element
                .GetSiblings()
                .OfType<HtmlElement>()
                .Any(e => string.Equals(e.Name, element.Name, StringComparison.OrdinalIgnoreCase));

        public override string ToString() => ":only-of-type";
    }
}