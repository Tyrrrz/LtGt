using System;
using LtGt.Models;

namespace LtGt.Internal.Selectors.Simple
{
    internal class IdSelector : Selector
    {
        public string Id { get; }

        public IdSelector(string id)
        {
            Id = id;
        }

        public override bool Matches(HtmlElement element) => string.Equals(element.GetId(), Id, StringComparison.Ordinal);

        public override string ToString() => $"#{Id}";
    }
}