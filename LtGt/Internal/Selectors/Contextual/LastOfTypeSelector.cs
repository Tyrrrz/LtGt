﻿using System;
using System.Linq;
using LtGt.Models;

namespace LtGt.Internal.Selectors.Contextual
{
    internal class LastOfTypeSelector : Selector
    {
        public Selector TargetSelector { get; }

        public LastOfTypeSelector(Selector targetSelector)
        {
            TargetSelector = targetSelector;
        }

        public override bool Matches(HtmlElement element) =>
            TargetSelector.Matches(element) && !element.GetNextSiblings().OfType<HtmlElement>()
                .Any(e => string.Equals(e.Name, element.Name, StringComparison.OrdinalIgnoreCase));

        public override string ToString() => ":last-of-type";
    }
}