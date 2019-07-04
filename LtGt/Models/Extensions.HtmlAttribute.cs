using System.Xml.Linq;
using LtGt.Internal;

namespace LtGt.Models
{
    public static partial class Extensions
    {
        /// <summary>
        /// Converts this <see cref="HtmlAttribute"/> to an instance of <see cref="XAttribute"/>.
        /// </summary>
        public static XAttribute ToXAttribute(this HtmlAttribute attribute)
        {
            attribute.GuardNotNull(nameof(attribute));

            return new XAttribute(attribute.Name, attribute.Value ?? "");
        }
    }
}