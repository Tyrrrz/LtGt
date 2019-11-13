using LtGt.Internal;
using LtGt.Models;

namespace LtGt
{
    /// <summary>
    /// HTML renderer.
    /// </summary>
    public partial class HtmlRenderer : IHtmlRenderer
    {
        /// <inheritdoc />
        public string RenderNode(HtmlNode node) => new HtmlStringBuilder().Append(node).ToString();
    }

    public partial class HtmlRenderer
    {
        /// <summary>
        /// Default instance of <see cref="HtmlRenderer"/>.
        /// </summary>
        public static HtmlRenderer Default { get; } = new HtmlRenderer();
    }
}