using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using LtGt.Internal;

namespace LtGt.Models
{
    public static partial class Extensions
    {
        /// <summary>
        /// Converts this <see cref="HtmlElement"/> to an instance of <see cref="XElement"/>.
        /// </summary>
        public static XElement ToXElement(this HtmlElement element)
        {
            element.GuardNotNull(nameof(element));

            var attributes = element.Attributes.Select(a => a.ToXAttribute()).ToArray<object>();
            var children = element.Children.Select(c => c.ToXNode()).ToArray<object>();

            return new XElement(element.Name, attributes.Concat(children).ToArray());
        }

        /// <summary>
        /// Gets an attribute of this <see cref="HtmlElement"/> that has specified name, or null if it's not found.
        /// Attribute name comparison is not case sensitive.
        /// </summary>
        public static HtmlAttribute GetAttribute(this HtmlElement element, string attributeName)
        {
            element.GuardNotNull(nameof(element));
            attributeName.GuardNotNull(nameof(attributeName));

            return element.Attributes.FirstOrDefault(a => string.Equals(a.Name, attributeName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets the value of 'id' attribute of this <see cref="HtmlElement"/> or null if it's not set.
        /// </summary>
        public static string GetId(this HtmlElement element)
        {
            element.GuardNotNull(nameof(element));

            return element.GetAttribute("id")?.Value;
        }

        /// <summary>
        /// Gets the value of 'class' attribute of this <see cref="HtmlElement"/> or null if it's not set.
        /// </summary>
        public static string GetClassName(this HtmlElement element)
        {
            element.GuardNotNull(nameof(element));

            return element.GetAttribute("class")?.Value;
        }

        /// <summary>
        /// Gets all individual whitespace-separated values of 'class' attribute of this <see cref="HtmlElement"/>.
        /// </summary>
        public static IReadOnlyList<string> GetClassNames(this HtmlElement element)
        {
            element.GuardNotNull(nameof(element));

            return element.GetClassName()?.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
        }

        /// <summary>
        /// Gets whether the value of 'class' attribute of this <see cref="HtmlElement"/> matches specified class name.
        /// Element class name comparison is case sensitive.
        /// </summary>
        public static bool MatchesClassName(this HtmlElement element, string className)
        {
            element.GuardNotNull(nameof(element));
            className.GuardNotNull(nameof(className));

            var elementClassNames = element.GetClassNames();
            var classNames = className.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

            return classNames.All(c => elementClassNames.Contains(c, StringComparer.Ordinal));
        }

        /// <summary>
        /// Gets the value of the 'href' attribute of this <see cref="HtmlElement"/> or null if it's not set.
        /// </summary>
        public static string GetHref(this HtmlElement element)
        {
            element.GuardNotNull(nameof(element));

            return element.GetAttribute("href")?.Value;
        }

        /// <summary>
        /// Gets the value of the 'src' attribute of this <see cref="HtmlElement"/> or null if it's not set.
        /// </summary>
        public static string GetSrc(this HtmlElement element)
        {
            element.GuardNotNull(nameof(element));

            return element.GetAttribute("src")?.Value;
        }
    }
}