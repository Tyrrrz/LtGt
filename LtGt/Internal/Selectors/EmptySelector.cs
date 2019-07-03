using System.Linq;
using LtGt.Models;

namespace LtGt.Internal.Selectors
{
    internal class EmptySelector : Selector
    {
        public override bool Matches(HtmlElement element) => !element.Children.Any();

        public override string ToString() => ":empty";
    }
}