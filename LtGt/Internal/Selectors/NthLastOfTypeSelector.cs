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

        public override bool Matches(HtmlElement element) =>
            NumberCompositionTerm.Check(element.GetNextSiblings().OfType<HtmlElement>()
                .Count(e => string.Equals(e.Name, element.Name, StringComparison.OrdinalIgnoreCase)));

        public override string ToString() => $":nth-last-of-type({NumberCompositionTerm})";
    }
}