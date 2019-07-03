using System;
using System.Linq;
using LtGt.Internal.Selectors.Terms;
using LtGt.Models;

namespace LtGt.Internal.Selectors
{
    internal class NthOfTypeSelector : Selector
    {
        public NumberCompositionTerm NumberCompositionTerm { get; }

        public NthOfTypeSelector(NumberCompositionTerm numberCompositionTerm)
        {
            NumberCompositionTerm = numberCompositionTerm;
        }

        public override bool Matches(HtmlElement element) =>
            NumberCompositionTerm.Check(element.GetPreviousSiblings().OfType<HtmlElement>()
                .Count(e => string.Equals(e.Name, element.Name, StringComparison.OrdinalIgnoreCase)));

        public override string ToString() => $"nth-of-type({NumberCompositionTerm})";
    }
}