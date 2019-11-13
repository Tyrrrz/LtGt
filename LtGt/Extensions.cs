using System.IO;
using LtGt.Models;

#if !NETSTANDARD1_0
using System.Net.Http;
using System.Threading.Tasks;
#endif

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
            var source = reader.ReadToEnd();
            return parser.ParseDocument(source);
        }

        /// <summary>
        /// Renders an HTML node to <see cref="TextWriter"/>.
        /// </summary>
        public static void SaveNode(this IHtmlRenderer renderer, HtmlNode node, TextWriter writer)
        {
            var source = renderer.RenderNode(node);
            writer.Write(source);
        }

#if !NETSTANDARD1_0
        /// <summary>
        /// Parses an HTML document from a file.
        /// </summary>
        public static HtmlDocument LoadDocument(this IHtmlParser parser, string filePath)
        {
            using var reader = File.OpenText(filePath);
            return parser.LoadDocument(reader);
        }

        /// <summary>
        /// Renders an HTML node to a file.
        /// </summary>
        public static void SaveNode(this IHtmlRenderer renderer, HtmlNode node, string filePath)
        {
            using var writer = File.CreateText(filePath);
            renderer.SaveNode(node, writer);
        }

        /// <summary>
        /// Reads HTTP content as an HTML document.
        /// </summary>
        public static async Task<HtmlDocument> ReadAsHtmlDocumentAsync(this HttpContent httpContent, IHtmlParser parser)
        {
            var source = await httpContent.ReadAsStringAsync().ConfigureAwait(false);
            return parser.ParseDocument(source);
        }

        /// <summary>
        /// Reads HTTP content as an HTML document.
        /// </summary>
        public static Task<HtmlDocument> ReadAsHtmlDocumentAsync(this HttpContent httpContent) =>
            httpContent.ReadAsHtmlDocumentAsync(HtmlParser.Default);

        /// <summary>
        /// Sends a GET request to the specified URI and returns response as an HTML document.
        /// </summary>
        public static async Task<HtmlDocument> GetHtmlDocumentAsync(this HttpClient httpClient, string requestUri, IHtmlParser parser)
        {
            using var response = await httpClient.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsHtmlDocumentAsync(parser);
        }

        /// <summary>
        /// Sends a GET request to the specified URI and returns response as an HTML document.
        /// </summary>
        public static Task<HtmlDocument> GetHtmlDocumentAsync(this HttpClient httpClient, string requestUri) =>
            httpClient.GetHtmlDocumentAsync(requestUri, HtmlParser.Default);
#endif
    }
}