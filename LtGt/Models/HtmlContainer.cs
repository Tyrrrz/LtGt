using System.Collections.Generic;

namespace LtGt.Models
{
    /// <summary>
    /// Represents an abstract container node in HTML syntax tree.
    /// </summary>
    public abstract class HtmlContainer : HtmlNode
    {
        /// <summary>
        /// Children inside of this container node.
        /// </summary>
        public IReadOnlyList<HtmlNode> Children { get; }

        /// <summary>
        /// Initializes an instance of <see cref="HtmlContainer"/>.
        /// </summary>
        protected HtmlContainer(IReadOnlyList<HtmlNode> children)
        {
            Children = children;
        }
    }
}