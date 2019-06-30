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
        /// Parses an HTML document from <see cref="TextReader"/>.
        /// </summary>
        public static HtmlDocument LoadDocument(this IHtmlParser parser, TextReader reader)
        {
            parser.GuardNotNull(nameof(parser));
            reader.GuardNotNull(nameof(reader));

            var source = reader.ReadToEnd();

            return parser.ParseDocument(source);
        }

        /// <summary>
        /// Renders an HTML node to <see cref="TextWriter"/>.
        /// </summary>
        public static void SaveNode(this IHtmlRenderer renderer, HtmlNode node, TextWriter writer)
        {
            renderer.GuardNotNull(nameof(renderer));
            node.GuardNotNull(nameof(node));
            writer.GuardNotNull(nameof(writer));

            var source = renderer.RenderNode(node);
            writer.Write(source);
        }

#if !NETSTANDARD1_0
        /// <summary>
        /// Parses an HTML document from a file.
        /// </summary>
        public static HtmlDocument LoadDocument(this IHtmlParser parser, string filePath)
        {
            parser.GuardNotNull(nameof(parser));
            filePath.GuardNotNull(nameof(filePath));

            using (var reader = File.OpenText(filePath))
                return parser.LoadDocument(reader);
        }

        /// <summary>
        /// Renders an HTML node to a file.
        /// </summary>
        public static void SaveNode(this IHtmlRenderer renderer, HtmlNode node, string filePath)
        {
            renderer.GuardNotNull(nameof(renderer));
            node.GuardNotNull(nameof(node));
            filePath.GuardNotNull(nameof(filePath));

            using (var writer = File.CreateText(filePath))
                renderer.SaveNode(node, writer);
        }
#endif
    }
}