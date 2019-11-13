using System.Xml.Linq;

namespace LtGt.Models
{
    public static partial class Extensions
    {
        /// <summary>
        /// Converts this <see cref="HtmlText"/> to an instance of <see cref="XText"/>.
        /// </summary>
        public static XText ToXText(this HtmlText text) => new XText(text.Value);
    }
}