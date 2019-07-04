using System.Linq;
using LtGt.Internal.Selectors.Terms;
using LtGt.Models;

namespace LtGt.Internal.Selectors
{
    internal class NthLastChildSelector : Selector
    {
        public NumberCompositionTerm NumberCompositionTerm { get; }

        public NthLastChildSelector(NumberCompositionTerm numberCompositionTerm)
        {
            NumberCompositionTerm = numberCompositionTerm;
        }

        public override bool Matches(HtmlElement element) => NumberCompositionTerm.Matches(element.GetNextSiblings().Count() + 1);

        public override string ToString() => $"nth-last-child({NumberCompositionTerm})";
    }
}