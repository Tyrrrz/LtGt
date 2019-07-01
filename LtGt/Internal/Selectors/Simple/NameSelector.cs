using System;
using LtGt.Models;

namespace LtGt.Internal.Selectors.Simple
{
    internal class NameSelector : Selector
    {
        public string Name { get; }

        public NameSelector(string name)
        {
            Name = name;
        }

        public override bool Matches(HtmlElement element) => string.Equals(element.Name, Name, StringComparison.OrdinalIgnoreCase);

        public override string ToString() => Name;
    }
}