using System.Xml.Linq;

namespace LtGt.Models
{
    public static partial class Extensions
    {
        /// <summary>
        /// Converts this <see cref="HtmlComment"/> to an instance of <see cref="XComment"/>.
        /// </summary>
        public static XComment ToXComment(this HtmlComment comment) => new XComment(comment.Value);
    }
}