using LtGt.Models;

namespace LtGt
{
    /// <summary>
    /// Provides an interface to render HTML objects to code.
    /// </summary>
    public interface IHtmlRenderer
    {
        /// <summary>
        /// Renders any <see cref="HtmlNode"/> to string.
        /// </summary>
        string RenderNode(HtmlNode node);
    }
}