using System;

namespace LtGt.Models
{
    /// <summary>
    /// Represents a comment node in HTML syntax tree.
    /// </summary>
    public class HtmlComment : HtmlNode, IEquatable<HtmlComment>
    {
        /// <summary>
        /// Text content of this comment node.
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// Initializes an instance of <see cref="HtmlComment"/>.
        /// </summary>
        public HtmlComment(string content)
        {
            Content = content;
        }

        /// <inheritdoc />
        public bool Equals(HtmlComment other)
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

            return Equals((HtmlComment) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode() => Content?.GetHashCode() ?? 0;

        /// <inheritdoc />
        public override string ToString() => $"<!-- {Content} -->";
    }
}