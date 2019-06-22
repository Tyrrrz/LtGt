using System;
using System.Collections.Generic;
using System.Linq;

namespace LtGt.Models
{
    /// <summary>
    /// Represents an abstract container node in HTML syntax tree.
    /// </summary>
    public abstract class HtmlContainer : HtmlNode, IEquatable<HtmlContainer>
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

        /// <inheritdoc />
        public bool Equals(HtmlContainer other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (ReferenceEquals(Children, other.Children))
                return true;

            return Children.SequenceEqual(other.Children);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals((HtmlContainer) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode() => Children?.GetHashCode() ?? 0;
    }
}