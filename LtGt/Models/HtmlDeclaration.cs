using LtGt.Internal;

namespace LtGt.Models
{
    /// <summary>
    /// Represents a declaration entity in HTML document object model.
    /// </summary>
    public partial class HtmlDeclaration : HtmlEntity
    {
        /// <summary>
        /// Name of this <see cref="HtmlDeclaration"/>.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Value of this <see cref="HtmlDeclaration"/>.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Initializes an instance of <see cref="HtmlDeclaration"/>.
        /// </summary>
        public HtmlDeclaration(string name, string value)
        {
            Name = name.GuardNotNull(nameof(name));
            Value = value.GuardNotNull(nameof(value));
        }

        /// <summary>
        /// Initializes an instance of <see cref="HtmlDeclaration"/>.
        /// </summary>
        public HtmlDeclaration(HtmlDeclaration other)
            : this(other.Name, other.Value)
        {
        }

        /// <inheritdoc />
        public override HtmlEntity Clone() => new HtmlDeclaration(this);

        /// <inheritdoc />
        public override string ToString() => $"<!{Name} {Value}>";
    }

    public partial class HtmlDeclaration
    {
        /// <summary>
        /// HTML document type declaration.
        /// </summary>
        public static HtmlDeclaration DoctypeHtml { get; } = new HtmlDeclaration("doctype", "html");
    }
}