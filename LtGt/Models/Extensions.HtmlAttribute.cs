using System.Xml.Linq;

namespace LtGt.Models
{
    public static partial class Extensions
    {
        /// <summary>
        /// Converts this <see cref="HtmlAttribute"/> to an instance of <see cref="XAttribute"/>.
        /// </summary>
        public static XAttribute ToXAttribute(this HtmlAttribute attribute) => new XAttribute(attribute.Name, attribute.Value ?? "");
    }
}