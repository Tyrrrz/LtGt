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

        public override bool Matches(HtmlElement element) =>
            element.Parent != null && NumberCompositionTerm.Check(element.Parent.Children.Count - element.Index);

        public override string ToString() => $"nth-last-child({NumberCompositionTerm})";
    }
}