using LtGt.Internal;

namespace LtGt.Models
{
    /// <summary>
    /// Represents an attribute node in HTML syntax tree.
    /// </summary>
    public class HtmlAttribute : HtmlNode
    {
        /// <summary>
        /// Name of this attribute node.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Value of this attribute node.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Initializes an instance of <see cref="HtmlAttribute"/>.
        /// </summary>
        public HtmlAttribute(string name, string value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Initializes an instance of <see cref="HtmlAttribute"/>.
        /// </summary>
        public HtmlAttribute(string name)
            : this(name, null)
        {
        }

        /// <inheritdoc />
        public override string ToString() => !Value.IsNullOrWhiteSpace() ? $"{Name}=\"{Value}\"" : Name;
    }
}