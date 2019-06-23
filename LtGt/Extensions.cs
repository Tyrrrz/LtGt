using System.IO;
using LtGt.Internal;
using LtGt.Models;

namespace LtGt
{
    /// <summary>
    /// Extensions for <see cref="LtGt"/>.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Parses an HTML document from source code provided by <see cref="TextReader"/>.
        /// </summary>
        public static HtmlDocument LoadDocument(this IHtmlParser parser, TextReader reader)
        {
            parser.GuardNotNull(nameof(parser));
            reader.GuardNotNull(nameof(reader));

            var source = reader.ReadToEnd();

            return parser.ParseDocument(source);
        }

        /// <summary>
        /// Parses an HTML node from source code provided by <see cref="TextReader"/>.
        /// </summary>
        public static HtmlNode LoadNode(this IHtmlParser parser, TextReader reader)
        {
            parser.GuardNotNull(nameof(parser));
            reader.GuardNotNull(nameof(reader));

            var source = reader.ReadToEnd();

            return parser.ParseNode(source);
        }

#if !NETSTANDARD1_0
        /// <summary>
        /// Parses an HTML document from source file.
        /// </summary>
        public static HtmlDocument LoadDocument(this IHtmlParser parser, string filePath)
        {
            parser.GuardNotNull(nameof(parser));
            filePath.GuardNotNull(nameof(filePath));

            using (var reader = File.OpenText(filePath))
                return parser.LoadDocument(reader);
        }

        /// <summary>
        /// Parses an HTML node from source file.
        /// </summary>
        public static HtmlNode LoadNode(this IHtmlParser parser, string filePath)
        {
            parser.GuardNotNull(nameof(parser));
            filePath.GuardNotNull(nameof(filePath));

            using (var reader = File.OpenText(filePath))
                return parser.LoadNode(reader);
        }
#endif
    }
}