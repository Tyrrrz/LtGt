namespace LtGt.Models
{
    /// <summary>
    /// Represents an abstract node in HTML document object model.
    /// </summary>
    public abstract class HtmlNode : HtmlEntity
    {
        /// <summary>
        /// Parent of this <see cref="HtmlNode"/>.
        /// </summary>
        public HtmlContainer Parent { get; internal set; }

        /// <summary>
        /// Previous sibling of this <see cref="HtmlNode"/>.
        /// </summary>
        public HtmlNode Previous { get; internal set; }

        /// <summary>
        /// Next sibling of this <see cref="HtmlNode"/>.
        /// </summary>
        public HtmlNode Next { get; internal set; }
    }
}