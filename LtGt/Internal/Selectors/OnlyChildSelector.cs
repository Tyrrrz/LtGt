using System.Linq;
using LtGt.Models;

namespace LtGt.Internal.Selectors
{
    internal class OnlyChildSelector : Selector
    {
        public override bool Matches(HtmlElement element) => !element.GetSiblings().Any();

        public override string ToString() => ":only-child";
    }
}