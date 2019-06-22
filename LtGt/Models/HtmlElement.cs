using System.Collections.Generic;

namespace LtGt.Models
{
    /// <summary>
    /// Represents an element node in HTML syntax tree.
    /// </summary>
    public class HtmlElement : HtmlContainer
    {
        /// <summary>
        /// Tag name of this element node.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Initializes an instance of <see cref="HtmlElement"/>.
        /// </summary>
        public HtmlElement(string name, IReadOnlyList<HtmlNode> children)
            : base(children)
        {
            Name = name;
        }

        /// <summary>
        /// Initializes an instance of <see cref="HtmlElement"/>.
        /// </summary>
        public HtmlElement(string name)
            : this(name, new HtmlNode[0])
        {
        }

        /// <inheritdoc />
        public override string ToString() => $"<{Name}>";
    }
}