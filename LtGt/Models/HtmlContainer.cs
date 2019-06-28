﻿using System.Collections.Generic;
using LtGt.Internal;

namespace LtGt.Models
{
    /// <summary>
    /// Represents an abstract container node in HTML document object model.
    /// </summary>
    public abstract class HtmlContainer : HtmlNode
    {
        /// <summary>
        /// Direct children of this container node.
        /// </summary>
        public IReadOnlyList<HtmlNode> Children { get; }

        /// <summary>
        /// Initializes an instance of <see cref="HtmlContainer"/>.
        /// </summary>
        protected HtmlContainer(IReadOnlyList<HtmlNode> children)
        {
            Children = children.GuardNotNull(nameof(children));
        }
    }
}