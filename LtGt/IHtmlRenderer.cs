using LtGt.Models;

namespace LtGt
{
    /// <summary>
    /// Provides an interface to render an abstract syntax tree into HTML.
    /// </summary>
    public interface IHtmlRenderer
    {
        /// <summary>
        /// Renders an HTML node to string.
        /// </summary>
        string RenderNode(HtmlNode node);
    }
}