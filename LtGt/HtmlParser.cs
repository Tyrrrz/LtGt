using LtGt.Internal;
using LtGt.Models;
using Sprache;

namespace LtGt
{
    /// <summary>
    /// HTML parser.
    /// </summary>
    public partial class HtmlParser : IHtmlParser
    {
        /// <inheritdoc />
        public HtmlDocument ParseDocument(string source) => HtmlGrammar.HtmlDocument.Parse(source);

        /// <inheritdoc />
        public HtmlNode ParseNode(string source) => HtmlGrammar.HtmlNode.Parse(source);
    }

    public partial class HtmlParser
    {
        /// <summary>
        /// Default instance of <see cref="HtmlParser"/>.
        /// </summary>
        public static IHtmlParser Default { get; } = new HtmlParser();
    }
}