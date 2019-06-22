using System.Collections.Generic;

namespace LtGt.Models
{
    /// <summary>
    /// Represents a document node in HTML syntax tree.
    /// </summary>
    public class HtmlDocument : HtmlContainer
    {
        /// <summary>
        /// Initializes an instance of <see cref="HtmlDocument"/>.
        /// </summary>
        public HtmlDocument(IReadOnlyList<HtmlNode> children)
            : base(children)
        {
        }
    }
}