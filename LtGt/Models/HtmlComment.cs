namespace LtGt.Models
{
    /// <summary>
    /// Represents a comment node in HTML syntax tree.
    /// </summary>
    public class HtmlComment : HtmlNode
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
        public override string ToString() => $"<!-- {Content} -->";
    }
}