using System.Collections.Generic;
using System.Linq;
using LtGt.Models;

namespace LtGt.Internal.Selectors.Simple
{
    internal class CombinedSelector : Selector
    {
        public IReadOnlyList<Selector> Selectors { get; }

        public CombinedSelector(IReadOnlyList<Selector> selectors)
        {
            Selectors = selectors;
        }

        public override bool Matches(HtmlElement element) => Selectors.All(s => s.Matches(element));

        public override string ToString() => string.Join("", Selectors);
    }
}