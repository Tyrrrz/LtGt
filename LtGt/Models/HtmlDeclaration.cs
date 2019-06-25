using System;
using LtGt.Internal;

namespace LtGt.Models
{
    /// <summary>
    /// Represents a declaration node in HTML syntax tree.
    /// </summary>
    public class HtmlDeclaration : HtmlNode, IEquatable<HtmlDeclaration>
    {
        /// <summary>
        /// Name of this declaration node.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Content of this declaration node.
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// Initializes an instance of <see cref="HtmlDeclaration"/>.
        /// </summary>
        public HtmlDeclaration(string name, string content)
        {
            Name = name.GuardNotNull(nameof(name));
            Content = content.GuardNotNull(nameof(content));
        }

        /// <inheritdoc />
        public bool Equals(HtmlDeclaration other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return string.Equals(Name, other.Name) &&
                   string.Equals(Content, other.Content);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals((HtmlDeclaration) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name?.GetHashCode() ?? 0) * 397) ^ (Content?.GetHashCode() ?? 0);
            }
        }

        /// <inheritdoc />
        public override string ToString() => $"<!{Name} {Content}>";
    }
}