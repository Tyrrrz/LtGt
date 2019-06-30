using System;
using System.Collections.Generic;
using System.Linq;
using LtGt.Internal;

namespace LtGt.Models
{
    /// <summary>
    /// Represents an element node in HTML document object model.
    /// </summary>
    public class HtmlElement : HtmlContainer, IEquatable<HtmlElement>
    {
        /// <summary>
        /// Tag name of this element node.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Attributes assigned to this element node.
        /// </summary>
        public IReadOnlyList<HtmlAttribute> Attributes { get; }

        /// <summary>
        /// Initializes an instance of <see cref="HtmlElement"/>.
        /// </summary>
        public HtmlElement(string name, IReadOnlyList<HtmlAttribute> attributes, IReadOnlyList<HtmlNode> children)
            : base(children)
        {
            Name = name.GuardNotNull(nameof(name));
            Attributes = attributes.GuardNotNull(nameof(attributes));
        }

        /// <summary>
        /// Initializes an instance of <see cref="HtmlElement"/>.
        /// </summary>
        public HtmlElement(string name, IReadOnlyList<HtmlAttribute> attributes)
            : this(name, attributes, new HtmlNode[0])
        {
        }

        /// <summary>
        /// Initializes an instance of <see cref="HtmlElement"/>.
        /// </summary>
        public HtmlElement(string name, IReadOnlyList<HtmlNode> children)
            : this(name, new HtmlAttribute[0], children)
        {
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
        public HtmlElement(string name, HtmlAttribute attribute, params HtmlNode[] children)
            : this(name, new[] {attribute}, children)
        {
        }

        /// <summary>
        /// Initializes an instance of <see cref="HtmlElement"/>.
        /// </summary>
        public HtmlElement(string name,
            HtmlAttribute attribute1, HtmlAttribute attribute2,
            params HtmlNode[] children)
            : this(name, new[] {attribute1, attribute2}, children)
        {
        }

        /// <summary>
        /// Initializes an instance of <see cref="HtmlElement"/>.
        /// </summary>
        public HtmlElement(string name,
            HtmlAttribute attribute1, HtmlAttribute attribute2, HtmlAttribute attribute3,
            params HtmlNode[] children)
            : this(name, new[] {attribute1, attribute2, attribute3}, children)
        {
        }

        /// <summary>
        /// Initializes an instance of <see cref="HtmlElement"/>.
        /// </summary>
        public HtmlElement(string name,
            HtmlAttribute attribute1, HtmlAttribute attribute2, HtmlAttribute attribute3, HtmlAttribute attribute4,
            params HtmlNode[] children)
            : this(name, new[] {attribute1, attribute2, attribute3, attribute4}, children)
        {
        }

        /// <summary>
        /// Initializes an instance of <see cref="HtmlElement"/>.
        /// </summary>
        public HtmlElement(string name)
            : this(name, new HtmlAttribute[0])
        {
        }

        /// <inheritdoc />
        public bool Equals(HtmlElement other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return string.Equals(Name, other.Name) &&
                   Attributes.SequenceEqual(other.Attributes) &&
                   Children.SequenceEqual(other.Children);
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
        public override int GetHashCode() => HashCode.Combine(Name, Attributes, Children);

        /// <inheritdoc />
        public override string ToString()
        {
            if (Attributes.Any() && Children.Any())
                return $"<{Name} ...>...</{Name}>";

            if (Attributes.Any())
                return $"<{Name} .../>";

            if (Children.Any())
                return $"<{Name}>...</{Name}>";

            return $"<{Name} />";
        }
    }
}