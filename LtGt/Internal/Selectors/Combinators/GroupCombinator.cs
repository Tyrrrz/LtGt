using System.Collections.Generic;
using System.Linq;
using System.Text;
using LtGt.Models;

namespace LtGt.Internal.Selectors.Combinators
{
    internal class GroupCombinator : Selector
    {
        public IReadOnlyList<Selector> Selectors { get; }

        public GroupCombinator(IReadOnlyList<Selector> selectors)
        {
            Selectors = selectors;
        }

        public override bool Matches(HtmlElement element) => Selectors.All(s => s.Matches(element));

        public override string ToString()
        {
            var buffer = new StringBuilder();

            foreach (var selector in Selectors)
                buffer.Append(selector);

            return buffer.ToString();
        }
    }
}