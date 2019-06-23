using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Gets all defined attributes.
        /// </summary>
        public static IEnumerable<HtmlAttribute> GetAttributes(this HtmlElement element)
        {
            element.GuardNotNull(nameof(element));

            return element.Children.OfType<HtmlAttribute>();
        }

        /// <summary>
        /// Gets an attribute with a given name or null if not defined.
        /// Attribute name comparison is not case sensitive.
        /// </summary>
        public static HtmlAttribute GetAttribute(this HtmlElement element, string name)
        {
            element.GuardNotNull(nameof(element));
            name.GuardNotNull(nameof(name));

            return element.GetAttributes().FirstOrDefault(a => string.Equals(a.Name, name, StringComparison.OrdinalIgnoreCase));
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
        /// Gets the individual classes in the 'class' attribute.
        /// </summary>
        public static IReadOnlyList<string> GetClassList(this HtmlElement element)
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

            var elementClassList = element.GetClassList();
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

        /// <summary>
        /// Gets inner text.
        /// </summary>
        public static string GetInnerText(this HtmlContainer container)
        {
            container.GuardNotNull(nameof(container));

            var textNodes = container.GetChildNodesRecursively().OfType<HtmlText>().ToArray();

            return textNodes.Length == 1
                ? textNodes.Single().Content.Trim()
                : textNodes.Select(n => n.Content).Concatenate().Trim();
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