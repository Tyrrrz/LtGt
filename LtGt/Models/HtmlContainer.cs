using System.Collections.Generic;
using System.Linq;
using LtGt.Internal;

namespace LtGt.Models
{
    /// <summary>
    /// Represents an abstract container node in HTML document object model.
    /// </summary>
    public abstract class HtmlContainer : HtmlNode
    {
        /// <summary>
        /// Direct children of this <see cref="HtmlContainer"/>.
        /// </summary>
        public IReadOnlyList<HtmlNode> Children { get; }

        /// <summary>
        /// Initializes an instance of <see cref="HtmlContainer"/>.
        /// </summary>
        protected HtmlContainer(IReadOnlyList<HtmlNode> children)
        {
            Children = children.GuardNotNull(nameof(children));

            // Update contextual information on children
            for (var i = 0; i < Children.Count; i++)
            {
                Children[i].Parent = this;
                Children[i].Previous = Children.ElementAtOrDefault(i - 1);
                Children[i].Next = Children.ElementAtOrDefault(i + 1);
            }
        }
    }
}