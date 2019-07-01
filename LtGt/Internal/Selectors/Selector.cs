using LtGt.Models;

namespace LtGt.Internal.Selectors
{
    internal abstract class Selector
    {
        public abstract bool Matches(HtmlElement element);
    }
}