namespace LtGt.Models
{
    /// <summary>
    /// Represents an abstract node in HTML document object model.
    /// </summary>
    public abstract class HtmlNode : HtmlEntity
    {
        /// <summary>
        /// Parent of this node.
        /// </summary>
        public HtmlContainer Parent { get; internal set; }

        /// <summary>
        /// Zero-based index of this node inside of its parent.
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// Next sibling of this node inside of its parent.
        /// </summary>
        public HtmlNode Next { get; internal set; }

        /// <summary>
        /// Previous sibling of this node inside of its parent.
        /// </summary>
        public HtmlNode Previous { get; internal set; }
    }
}