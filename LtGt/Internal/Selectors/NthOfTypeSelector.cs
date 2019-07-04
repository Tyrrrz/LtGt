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
            var previousSiblingsOfSameType = element.GetPreviousSiblings()
                .OfType<HtmlElement>()
                .Where(e => string.Equals(e.Name, element.Name, StringComparison.OrdinalIgnoreCase))
                .ToArray();

            var index = previousSiblingsOfSameType.Length + 1;

            return NumberCompositionTerm.Check(index);
        }

        public override string ToString() => $"nth-of-type({NumberCompositionTerm})";
    }
}