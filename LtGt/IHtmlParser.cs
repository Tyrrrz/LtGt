using LtGt.Models;

namespace LtGt
{
    /// <summary>
    /// Provides an interface to parse HTML into an object model representation.
    /// </summary>
    public interface IHtmlParser
    {
        /// <summary>
        /// Parses an HTML document from input.
        /// </summary>
        HtmlDocument ParseDocument(string source);

        /// <summary>
        /// Parses an HTML node from input.
        /// </summary>
        HtmlNode ParseNode(string source);
    }
}