using LtGt.Internal;

namespace LtGt.Models
{
    /// <summary>
    /// Represents a declaration node in HTML syntax tree.
    /// </summary>
    public class HtmlDeclaration : HtmlNode
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
            Name = name;
            Content = content;
        }

        /// <inheritdoc />
        public override string ToString() => !Content.IsNullOrWhiteSpace() ? $"Name: {Content}" : Name;
    }
}