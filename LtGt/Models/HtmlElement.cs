using System;
using System.Collections.Generic;

namespace LtGt.Models
{
    /// <summary>
    /// Represents an element node in HTML syntax tree.
    /// </summary>
    public class HtmlElement : HtmlContainer, IEquatable<HtmlElement>
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
        public HtmlElement(string name, params HtmlNode[] children)
            : this(name, (IReadOnlyList<HtmlNode>) children)
        {
        }

        /// <summary>
        /// Initializes an instance of <see cref="HtmlElement"/>.
        /// </summary>
        public HtmlElement(string name)
            : this(name, new HtmlNode[0])
        {
        }

        /// <inheritdoc />
        public bool Equals(HtmlElement other)
        {
            if (!base.Equals(other)) return false;
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return string.Equals(Name, other.Name);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals((HtmlElement) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode() => Name?.GetHashCode() ?? 0;

        /// <inheritdoc />
        public override string ToString() => $"<{Name}>";
    }
}