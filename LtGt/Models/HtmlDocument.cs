using System.Collections.Generic;
using System.Linq;
using LtGt.Internal;

namespace LtGt.Models
{
    /// <summary>
    /// Represents a document node in HTML document object model.
    /// </summary>
    public class HtmlDocument : HtmlContainer
    {
        /// <summary>
        /// Declaration of this <see cref="HtmlDocument"/>.
        /// </summary>
        public HtmlDeclaration Declaration { get; }

        /// <summary>
        /// Initializes an instance of <see cref="HtmlDocument"/>.
        /// </summary>
        public HtmlDocument(HtmlDeclaration declaration, IReadOnlyList<HtmlNode> children)
            : base(children)
        {
            Declaration = declaration.GuardNotNull(nameof(declaration));
        }

        /// <summary>
        /// Initializes an instance of <see cref="HtmlDocument"/>.
        /// </summary>
        public HtmlDocument(HtmlDeclaration declaration, params HtmlNode[] children)
            : this(declaration, (IReadOnlyList<HtmlNode>) children)
        {
        }

        /// <summary>
        /// Initializes an instance of <see cref="HtmlDocument"/>.
        /// </summary>
        public HtmlDocument(HtmlDocument other)
            : this((HtmlDeclaration) other.Declaration.Clone(),
                other.Children.Select(n => (HtmlNode) n.Clone()).ToArray())
        {
        }

        /// <inheritdoc />
        public override HtmlEntity Clone() => new HtmlDocument(this);

        /// <inheritdoc />
        public override string ToString()
        {
            if (Children.Any())
                return $"{Declaration} ...";

            return $"{Declaration}";
        }
    }
}