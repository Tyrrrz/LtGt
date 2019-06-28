using System;
using System.Collections.Generic;
using System.Linq;
using LtGt.Internal;

namespace LtGt.Models
{
    /// <summary>
    /// Represents a document node in HTML document object model.
    /// </summary>
    public class HtmlDocument : HtmlContainer, IEquatable<HtmlDocument>
    {
        /// <summary>
        /// HTML declaration of this document.
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

        /// <inheritdoc />
        public bool Equals(HtmlDocument other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Declaration.Equals(other.Declaration) && Children.SequenceEqual(other.Children);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals((HtmlDocument) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeBuilder.Combine(Declaration, Children);
    }
}