using System;
using LtGt.Internal;

namespace LtGt.Models
{
    /// <summary>
    /// Represents a text node in HTML syntax tree.
    /// </summary>
    public class HtmlText : HtmlNode, IEquatable<HtmlText>
    {
        /// <summary>
        /// Content of this text node.
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// Initializes an instance of <see cref="HtmlText"/>.
        /// </summary>
        public HtmlText(string content)
        {
            Content = content.GuardNotNull(nameof(content));
        }

        /// <inheritdoc />
        public bool Equals(HtmlText other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return string.Equals(Content, other.Content);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals((HtmlText) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode() => Content?.GetHashCode() ?? 0;

        /// <inheritdoc />
        public override string ToString() => Content;
    }
}