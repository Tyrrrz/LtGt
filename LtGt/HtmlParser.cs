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
        public HtmlDocument ParseDocument(string source)
        {
            source.GuardNotNull(nameof(source));

            return HtmlGrammar.HtmlDocument.Parse(source);
        }

        /// <inheritdoc />
        public HtmlElement ParseElement(string source)
        {
            source.GuardNotNull(nameof(source));

            return HtmlGrammar.HtmlElement.Parse(source);
        }

        /// <inheritdoc />
        public HtmlNode ParseNode(string source)
        {
            source.GuardNotNull(nameof(source));

            return HtmlGrammar.HtmlNode.Parse(source);
        }
    }

    public partial class HtmlParser
    {
        /// <summary>
        /// Default instance of <see cref="HtmlParser"/>.
        /// </summary>
        public static HtmlParser Default { get; } = new HtmlParser();
    }
}