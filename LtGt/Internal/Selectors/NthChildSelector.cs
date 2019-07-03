using LtGt.Internal.Selectors.Terms;
using LtGt.Models;

namespace LtGt.Internal.Selectors
{
    internal class NthChildSelector : Selector
    {
        public NumberCompositionTerm NumberCompositionTerm { get; }

        public NthChildSelector(NumberCompositionTerm numberCompositionTerm)
        {
            NumberCompositionTerm = numberCompositionTerm;
        }

        public override bool Matches(HtmlElement element) => NumberCompositionTerm.Check(element.Index);

        public override string ToString() => $":nth-child({NumberCompositionTerm})";
    }
}