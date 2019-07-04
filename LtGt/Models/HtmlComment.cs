using LtGt.Internal;

namespace LtGt.Models
{
    /// <summary>
    /// Represents a comment node in HTML document object model.
    /// </summary>
    public class HtmlComment : HtmlNode
    {
        /// <summary>
        /// Text content of this <see cref="HtmlComment"/>.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Initializes an instance of <see cref="HtmlComment"/>.
        /// </summary>
        public HtmlComment(string value)
        {
            Value = value.GuardNotNull(nameof(value));
        }

        /// <summary>
        /// Initializes an instance of <see cref="HtmlComment"/>.
        /// </summary>
        public HtmlComment(HtmlComment other)
            : this(other.Value)
        {
        }

        /// <inheritdoc />
        public override HtmlEntity Clone() => new HtmlComment(this);

        /// <inheritdoc />
        public override string ToString() => $"<!-- {Value} -->";
    }
}