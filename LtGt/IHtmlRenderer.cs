using LtGt.Models;

namespace LtGt
{
    /// <summary>
    /// Provides an interface to render HTML from its object model representation.
    /// </summary>
    public interface IHtmlRenderer
    {
        /// <summary>
        /// Renders an HTML node to string.
        /// </summary>
        string RenderNode(HtmlNode node);
    }
}