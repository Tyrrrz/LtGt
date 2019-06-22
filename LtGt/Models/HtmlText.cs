namespace LtGt.Models
{
    /// <summary>
    /// Represents a text node in HTML syntax tree.
    /// </summary>
    public class HtmlText : HtmlNode
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
            Content = content;
        }

        /// <inheritdoc />
        public override string ToString() => Content;
    }
}