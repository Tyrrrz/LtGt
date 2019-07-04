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

        public override bool Matches(HtmlElement element)
        {
            var previousSiblingsOfSameTypeCount = element
                .GetPreviousSiblings()
                .OfType<HtmlElement>()
                .Count(e => string.Equals(e.Name, element.Name, StringComparison.OrdinalIgnoreCase));

            return NumberCompositionTerm.Matches(previousSiblingsOfSameTypeCount + 1);
        }

        public override string ToString() => $"nth-of-type({NumberCompositionTerm})";
    }
}