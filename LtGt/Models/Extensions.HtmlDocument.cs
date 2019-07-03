using System.Linq;
using System.Xml.Linq;
using LtGt.Internal;

namespace LtGt.Models
{
    public static partial class Extensions
    {
        /// <summary>
        /// Converts this <see cref="HtmlDocument"/> to an instance of <see cref="XDocument"/>.
        /// </summary>
        public static XDocument ToXDocument(this HtmlDocument document)
        {
            document.GuardNotNull(nameof(document));

            var children = document.Children.Select(c => c.ToXNode()).ToArray<object>();

            return new XDocument(children);
        }

        /// <summary>
        /// Gets the 'head' element inside this <see cref="HtmlDocument"/> or null if it's not found.
        /// </summary>
        public static HtmlElement GetHead(this HtmlDocument document)
        {
            document.GuardNotNull(nameof(document));

            return document.GetElementsByTagName("head").FirstOrDefault();
        }

        /// <summary>
        /// Gets the 'body' element inside this <see cref="HtmlDocument"/> or null if it's not found.
        /// </summary>
        public static HtmlElement GetBody(this HtmlDocument document)
        {
            document.GuardNotNull(nameof(document));

            return document.GetElementsByTagName("body").FirstOrDefault();
        }

        /// <summary>
        /// Gets the title of this <see cref="HtmlDocument"/> or null if it's not set.
        /// </summary>
        public static string GetTitle(this HtmlDocument document)
        {
            document.GuardNotNull(nameof(document));

            return document.GetHead()?.GetElementsByTagName("title").FirstOrDefault()?.GetInnerText();
        }
    }
}