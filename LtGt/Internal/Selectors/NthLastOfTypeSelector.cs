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
            var nextSiblingsOfSameType = element.GetNextSiblings()
                .OfType<HtmlElement>()
                .Where(e => string.Equals(e.Name, element.Name, StringComparison.OrdinalIgnoreCase))
                .ToArray();

            var index = nextSiblingsOfSameType.Length + 1;

            return NumberCompositionTerm.Check(index);
        }

        public override string ToString() => $":nth-last-of-type({NumberCompositionTerm})";
    }
}