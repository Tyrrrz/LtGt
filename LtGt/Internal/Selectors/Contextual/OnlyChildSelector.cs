using System.Linq;
using LtGt.Models;

namespace LtGt.Internal.Selectors.Contextual
{
    internal class OnlyChildSelector : Selector
    {
        public Selector TargetSelector { get; }

        public OnlyChildSelector(Selector targetSelector)
        {
            TargetSelector = targetSelector;
        }

        public override bool Matches(HtmlElement element) =>
            TargetSelector.Matches(element) && !element.GetSiblings().Any();

        public override string ToString() => ":only-child";
    }
}