using LtGt.Internal;

namespace LtGt.Models
{
    /// <summary>
    /// Represents a text node in HTML document object model.
    /// </summary>
    public class HtmlText : HtmlNode
    {
        /// <summary>
        /// Text content of this <see cref="HtmlText"/>.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Initializes an instance of <see cref="HtmlText"/>.
        /// </summary>
        public HtmlText(string value)
        {
            Value = value.GuardNotNull(nameof(value));
        }

        /// <summary>
        /// Initializes an instance of <see cref="HtmlText"/>.
        /// </summary>
        public HtmlText(HtmlText other)
            : this(other.Value)
        {
        }

        /// <inheritdoc />
        public override HtmlEntity Clone() => new HtmlText(this);

        /// <inheritdoc />
        public override string ToString() => Value;
    }
}