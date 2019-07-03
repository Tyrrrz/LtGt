using System;
using LtGt.Models;

namespace LtGt.Internal.Selectors
{
    internal class TypeSelector : Selector
    {
        public string Name { get; }

        public TypeSelector(string name)
        {
            Name = name;
        }

        public override bool Matches(HtmlElement element) => string.Equals(element.Name, Name, StringComparison.OrdinalIgnoreCase);

        public override string ToString() => Name;
    }
}