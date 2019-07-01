using LtGt.Models;

namespace LtGt
{
    /// <summary>
    /// Provides an interface to parse HTML into objects.
    /// </summary>
    public interface IHtmlParser
    {
        /// <summary>
        /// Parses <see cref="HtmlDocument"/> from input.
        /// </summary>
        HtmlDocument ParseDocument(string source);

        /// <summary>
        /// Parses <see cref="HtmlElement"/> from input.
        /// </summary>
        HtmlElement ParseElement(string source);

        /// <summary>
        /// Parses any <see cref="HtmlNode"/> from input.
        /// </summary>
        HtmlNode ParseNode(string source);
    }
}