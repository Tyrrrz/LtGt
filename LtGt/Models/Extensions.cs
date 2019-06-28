using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LtGt.Internal;

namespace LtGt.Models
{
    /// <summary>
    /// Extensions for <see cref="Models"/>.
    /// </summary>
    public static partial class Extensions
    {
    }

    // HtmlElement
    public static partial class Extensions
    {
        /// <summary>
        /// Gets an attribute with a given name or null if not defined.
        /// Attribute name comparison is not case sensitive.
        /// </summary>
        public static HtmlAttribute GetAttribute(this HtmlElement element, string name)
        {
            element.GuardNotNull(nameof(element));
            name.GuardNotNull(nameof(name));

            return element.Attributes.FirstOrDefault(a => string.Equals(a.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets the value of the 'id' attribute or null if not defined.
        /// </summary>
        public static string GetId(this HtmlElement element)
        {
            element.GuardNotNull(nameof(element));

            return element.GetAttribute("id")?.Value;
        }

        /// <summary>
        /// Gets the value of the 'class' attribute or null if not defined.
        /// </summary>
        public static string GetClassName(this HtmlElement element)
        {
            element.GuardNotNull(nameof(element));

            return element.GetAttribute("class")?.Value;
        }

        /// <summary>
        /// Gets the individual class names in the 'class' attribute separated by whitespace.
        /// </summary>
        public static IReadOnlyList<string> GetClassNames(this HtmlElement element)
        {
            element.GuardNotNull(nameof(element));

            return element.GetClassName()?.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
        }

        /// <summary>
        /// Gets whether the value of the 'class' attribute is matched by the given class name.
        /// Element class name comparison is case sensitive.
        /// </summary>
        public static bool MatchesClassName(this HtmlElement element, string className)
        {
            element.GuardNotNull(nameof(element));
            className.GuardNotNull(nameof(className));

            var elementClassList = element.GetClassNames();
            var targetClassList = className.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

            return targetClassList.All(c => elementClassList.Contains(c, StringComparer.Ordinal));
        }

        /// <summary>
        /// Gets the value of the 'href' attribute or null if not defined.
        /// </summary>
        public static string GetHref(this HtmlElement element)
        {
            element.GuardNotNull(nameof(element));

            return element.GetAttribute("href")?.Value;
        }

        /// <summary>
        /// Gets the value of the 'src' attribute or null if not defined.
        /// </summary>
        public static string GetSrc(this HtmlElement element)
        {
            element.GuardNotNull(nameof(element));

            return element.GetAttribute("src")?.Value;
        }
    }

    // HtmlContainer
    public static partial class Extensions
    {
        /// <summary>
        /// Gets all child nodes as they appear in document order.
        /// </summary>
        public static IEnumerable<HtmlNode> GetChildNodesRecursively(this HtmlContainer container)
        {
            container.GuardNotNull(nameof(container));

            foreach (var child in container.Children)
            {
                yield return child;

                if (child is HtmlContainer containerChild)
                {
                    foreach (var recursiveChild in containerChild.GetChildNodesRecursively())
                        yield return recursiveChild;
                }
            }
        }

        /// <summary>
        /// Gets all child element nodes as they appear in document order.
        /// </summary>
        public static IEnumerable<HtmlElement> GetChildElementsRecursively(this HtmlContainer container)
        {
            container.GuardNotNull(nameof(container));

            return container.GetChildNodesRecursively().OfType<HtmlElement>();
        }

        /// <summary>
        /// Gets an element that has given ID or null if not found.
        /// Element ID comparison is case sensitive.
        /// </summary>
        public static HtmlElement GetElementById(this HtmlContainer container, string id)
        {
            container.GuardNotNull(nameof(container));
            id.GuardNotNull(nameof(id));

            return container.GetChildElementsRecursively().FirstOrDefault(e => string.Equals(e.GetId(), id, StringComparison.Ordinal));
        }

        /// <summary>
        /// Gets elements that have given tag name.
        /// Element tag name comparison is not case sensitive.
        /// </summary>
        public static IEnumerable<HtmlElement> GetElementsByTagName(this HtmlContainer container, string tagName)
        {
            container.GuardNotNull(nameof(container));
            tagName.GuardNotNull(nameof(tagName));

            if (string.Equals(tagName, "*", StringComparison.OrdinalIgnoreCase))
                return container.GetChildElementsRecursively();

            return container.GetChildElementsRecursively().Where(e => string.Equals(e.Name, tagName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets an element that has given tag name or null if not found.
        /// Element tag name comparison is not case sensitive.
        /// </summary>
        public static HtmlElement GetElementByTagName(this HtmlContainer container, string tagName)
        {
            container.GuardNotNull(nameof(container));
            tagName.GuardNotNull(nameof(tagName));

            return container.GetElementsByTagName(tagName).FirstOrDefault();
        }

        /// <summary>
        /// Gets elements that match given class name.
        /// Element class name comparison is case sensitive.
        /// </summary>
        public static IEnumerable<HtmlElement> GetElementsByClassName(this HtmlContainer container, string className)
        {
            container.GuardNotNull(nameof(container));
            className.GuardNotNull(nameof(className));

            return container.GetChildElementsRecursively().Where(e => e.MatchesClassName(className));
        }

        /// <summary>
        /// Gets an element that matches given class name or null if not found.
        /// Element class name comparison is case sensitive.
        /// </summary>
        public static HtmlElement GetElementByClassName(this HtmlContainer container, string className)
        {
            container.GuardNotNull(nameof(container));
            className.GuardNotNull(nameof(className));

            return container.GetElementsByClassName(className).FirstOrDefault();
        }

        private static string GetTextRepresentation(this HtmlElement element, bool isFirstNode)
        {
            if (string.Equals(element.Name, "script", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(element.Name, "style", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(element.Name, "select", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(element.Name, "canvas", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(element.Name, "video", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(element.Name, "iframe", StringComparison.OrdinalIgnoreCase))
                return "";

            if (string.Equals(element.Name, "br", StringComparison.OrdinalIgnoreCase))
                return Environment.NewLine;

            var shouldPrependNewLine =
                !isFirstNode &&
                (string.Equals(element.Name, "p", StringComparison.OrdinalIgnoreCase) ||
                 string.Equals(element.Name, "caption", StringComparison.OrdinalIgnoreCase) ||
                 string.Equals(element.Name, "div", StringComparison.OrdinalIgnoreCase) ||
                 string.Equals(element.Name, "li", StringComparison.OrdinalIgnoreCase));

            if (shouldPrependNewLine)
                return Environment.NewLine + element.GetTextRepresentation();

            return element.GetTextRepresentation();
        }

        private static string GetTextRepresentation(this HtmlContainer container)
        {
            var buffer = new StringBuilder();

            foreach (var child in container.Children)
            {
                if (child is HtmlText childText)
                {
                    buffer.Append(childText.Content);
                }
                else if (child is HtmlElement childElement)
                {
                    buffer.Append(childElement.GetTextRepresentation(buffer.Length == 0));
                }
            }

            return buffer.ToString();
        }

        /// <summary>
        /// Gets the child nodes formatted as text.
        /// </summary>
        public static string GetInnerText(this HtmlContainer container)
        {
            container.GuardNotNull(nameof(container));

            return container.GetTextRepresentation();
        }
    }

    // HtmlDocument
    public static partial class Extensions
    {
        /// <summary>
        /// Gets the 'head' element or null if not found.
        /// </summary>
        public static HtmlElement GetHead(this HtmlDocument document)
        {
            document.GuardNotNull(nameof(document));

            return document.GetElementByTagName("head");
        }

        /// <summary>
        /// Gets the 'body' element or null if not found.
        /// </summary>
        public static HtmlElement GetBody(this HtmlDocument document)
        {
            document.GuardNotNull(nameof(document));

            return document.GetElementByTagName("body");
        }

        /// <summary>
        /// Gets the title or null if not defined.
        /// </summary>
        public static string GetTitle(this HtmlDocument document)
        {
            document.GuardNotNull(nameof(document));

            return document.GetHead()?.GetElementByTagName("title")?.GetInnerText();
        }
    }
}