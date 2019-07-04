using System;
using System.Linq;
using LtGt.Internal.Selectors.Terms;
using LtGt.Models;

namespace LtGt.Internal.Selectors
{
    internal class NthLastOfTypeSelector : Selector
    {
        public NumberCompositionTerm NumberCompositionTerm { get; }

        public NthLastOfTypeSelector(NumberCompositionTerm numberCompositionTerm)
        {
            NumberCompositionTerm = numberCompositionTerm;
        }

        public override bool Matches(HtmlElement element)
        {
            var nextSiblingsOfSameTypeCount = element
                .GetNextSiblings()
                .OfType<HtmlElement>()
                .Count(e => string.Equals(e.Name, element.Name, StringComparison.OrdinalIgnoreCase));

            return NumberCompositionTerm.Matches(nextSiblingsOfSameTypeCount + 1);
        }

        public override string ToString() => $":nth-last-of-type({NumberCompositionTerm})";
    }
}