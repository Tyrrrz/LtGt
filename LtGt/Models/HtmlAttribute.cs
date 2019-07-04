using LtGt.Internal;

namespace LtGt.Models
{
    /// <summary>
    /// Represents an attribute entity in HTML document object model.
    /// </summary>
    public class HtmlAttribute : HtmlEntity
    {
        /// <summary>
        /// Name of this <see cref="HtmlAttribute"/>.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Value of this <see cref="HtmlAttribute"/>.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Parent of this <see cref="HtmlAttribute"/>.
        /// </summary>
        public HtmlElement Parent { get; internal set; }

        /// <summary>
        /// Previous sibling of this <see cref="HtmlAttribute"/>.
        /// </summary>
        public HtmlAttribute Previous { get; internal set; }

        /// <summary>
        /// Next sibling of this <see cref="HtmlAttribute"/>.
        /// </summary>
        public HtmlAttribute Next { get; internal set; }

        /// <summary>
        /// Initializes an instance of <see cref="HtmlAttribute"/>.
        /// </summary>
        public HtmlAttribute(string name, string value)
        {
            Name = name.GuardNotNull(nameof(name));
            Value = value; // value can be null
        }

        /// <summary>
        /// Initializes an instance of <see cref="HtmlAttribute"/>.
        /// </summary>
        public HtmlAttribute(string name)
            : this(name, null)
        {
        }

        /// <summary>
        /// Initializes an instance of <see cref="HtmlAttribute"/>.
        /// </summary>
        public HtmlAttribute(HtmlAttribute other)
            : this(other.Name, other.Value)
        {
        }

        /// <inheritdoc />
        public override HtmlEntity Clone() => new HtmlAttribute(this);

        /// <inheritdoc />
        public override string ToString() => !Value.IsNullOrWhiteSpace() ? $"{Name}=\"{Value}\"" : Name;
    }
}