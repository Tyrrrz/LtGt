using LtGt.Models;

namespace LtGt
{
    /// <summary>
    /// Interface for <see cref="HtmlParser"/>.
    /// </summary>
    public interface IHtmlParser
    {
        /// <summary>
        /// Parses an HTML document from input source code.
        /// </summary>
        HtmlDocument ParseDocument(string source);

        /// <summary>
        /// Parses an HTML node from input source code.
        /// </summary>
        HtmlNode ParseNode(string source);
    }
}