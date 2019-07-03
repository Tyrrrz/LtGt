using LtGt.Models;

namespace LtGt.Internal.Selectors
{
    internal class NotSelector : Selector
    {
        public Selector TargetSelector { get; }

        public NotSelector(Selector targetSelector)
        {
            TargetSelector = targetSelector;
        }

        public override bool Matches(HtmlElement element) => !TargetSelector.Matches(element);

        public override string ToString() => $":not({TargetSelector})";
    }
}