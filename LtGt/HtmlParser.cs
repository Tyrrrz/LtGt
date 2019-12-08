using LtGt.Internal;
using LtGt.Models;
using LtGt.Parser;
using Sprache;

namespace LtGt
{
    /// <summary>
    /// HTML parser.
    /// </summary>
    public partial class HtmlParser : IHtmlParser
    {
        /// <inheritdoc />
        public HtmlDocument ParseDocument(string source) => Parser.HtmlParser.ParseDocument(source);

        /// <inheritdoc />
        public HtmlElement ParseElement(string source) => Parser.HtmlParser.ParseElement(source);

        /// <inheritdoc />
        public HtmlNode ParseNode(string source) => Parser.HtmlParser.ParseNode(source);
    }

    public partial class HtmlParser
    {
        /// <summary>
        /// Default instance of <see cref="HtmlParser"/>.
        /// </summary>
        public static HtmlParser Default { get; } = new HtmlParser();
    }
}