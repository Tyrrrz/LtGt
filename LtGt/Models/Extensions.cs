﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using LtGt.Internal;
using LtGt.Internal.Selectors;
using Sprache;

namespace LtGt.Models
{
    /// <summary>
    /// Extensions for <see cref="Models"/>.
    /// </summary>
    public static partial class Extensions
    {
    }

    // HtmlAttribute
    public static partial class Extensions
    {
        /// <summary>
        /// Converts <see cref="HtmlAttribute"/> to <see cref="XAttribute"/>.
        /// </summary>
        public static XAttribute ToXAttribute(this HtmlAttribute attribute)
        {
            attribute.GuardNotNull(nameof(attribute));

            return new XAttribute(attribute.Name, attribute.Value ?? "");
        }
    }

    // HtmlComment
    public static partial class Extensions
    {
        /// <summary>
        /// Converts <see cref="HtmlComment"/> to <see cref="XComment"/>.
        /// </summary>
        public static XComment ToXComment(this HtmlComment comment)
        {
            comment.GuardNotNull(nameof(comment));

            return new XComment(comment.Content);
        }
    }

    // HtmlText
    public static partial class Extensions
    {
        /// <summary>
        /// Converts <see cref="HtmlText"/> to <see cref="XText"/>.
        /// </summary>
        public static XText ToXText(this HtmlText text)
        {
            text.GuardNotNull(nameof(text));

            return new XText(text.Content);
        }
    }

    // HtmlElement
    public static partial class Extensions
    {
        /// <summary>
        /// Converts <see cref="HtmlElement"/> to <see cref="XElement"/>.
        /// </summary>
        public static XElement ToXElement(this HtmlElement element)
        {
            element.GuardNotNull(nameof(element));

            var attributes = element.Attributes.Select(a => a.ToXAttribute()).ToArray<object>();
            var children = element.Children.Select(c => c.ToXNode()).ToArray<object>();

            return new XElement(element.Name, attributes.Concat(children).ToArray());
        }

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
        public static IEnumerable<HtmlElement> GetChildElements(this HtmlContainer container)
        {
            container.GuardNotNull(nameof(container));

            return container.Children.OfType<HtmlElement>();
        }

        /// <summary>
        /// Gets all child nodes as they appear in document order.
        /// </summary>
        public static IEnumerable<HtmlNode> GetDescendants(this HtmlContainer container)
        {
            container.GuardNotNull(nameof(container));

            foreach (var child in container.Children)
            {
                yield return child;

                if (child is HtmlContainer containerChild)
                {
                    foreach (var recursiveChild in containerChild.GetDescendants())
                        yield return recursiveChild;
                }
            }
        }

        /// <summary>
        /// Gets all child element nodes as they appear in document order.
        /// </summary>
        public static IEnumerable<HtmlElement> GetDescendantElements(this HtmlContainer container)
        {
            container.GuardNotNull(nameof(container));

            return container.GetDescendants().OfType<HtmlElement>();
        }

        /// <summary>
        /// Gets an element that has given ID or null if not found.
        /// Element ID comparison is case sensitive.
        /// </summary>
        public static HtmlElement GetElementById(this HtmlContainer container, string id)
        {
            container.GuardNotNull(nameof(container));
            id.GuardNotNull(nameof(id));

            return container.GetDescendantElements().FirstOrDefault(e => string.Equals(e.GetId(), id, StringComparison.Ordinal));
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
                return container.GetDescendantElements();

            return container.GetDescendantElements().Where(e => string.Equals(e.Name, tagName, StringComparison.OrdinalIgnoreCase));
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

            return container.GetDescendantElements().Where(e => e.MatchesClassName(className));
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
        /// Gets elements that match given CSS selector.
        /// </summary>
        public static IEnumerable<HtmlElement> GetElementsBySelector(this HtmlContainer container, string selector)
        {
            container.GuardNotNull(nameof(container));
            selector.GuardNotNull(nameof(selector));

            if (string.Equals(selector, "*", StringComparison.OrdinalIgnoreCase))
                return container.GetDescendantElements();

            var selectorParseResult = SelectorGrammar.Selector.TryParse(selector);

            if (!selectorParseResult.WasSuccessful)
                return Enumerable.Empty<HtmlElement>();

            return container.GetDescendantElements().Where(e => selectorParseResult.Value.Matches(e));
        }

        /// <summary>
        /// Gets an element that matches given CSS selector or null if not found.
        /// </summary>
        public static HtmlElement GetElementBySelector(this HtmlContainer container, string selector)
        {
            container.GuardNotNull(nameof(container));
            selector.GuardNotNull(nameof(selector));

            return container.GetElementsBySelector(selector).FirstOrDefault();
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
        /// Converts <see cref="HtmlDocument"/> to <see cref="XDocument"/>.
        /// </summary>
        public static XDocument ToXDocument(this HtmlDocument document)
        {
            document.GuardNotNull(nameof(document));

            var children = document.Children.Select(c => c.ToXNode()).ToArray<object>();

            return new XDocument(children);
        }

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

    // HtmlNode
    public static partial class Extensions
    {
        /// <summary>
        /// Converts <see cref="HtmlNode"/> to <see cref="XNode"/>.
        /// </summary>
        public static XNode ToXNode(this HtmlNode node)
        {
            node.GuardNotNull(nameof(node));

            if (node is HtmlComment comment)
                return comment.ToXComment();

            if (node is HtmlText text)
                return text.ToXText();

            if (node is HtmlElement element)
                return element.ToXElement();

            if (node is HtmlDocument document)
                return document.ToXDocument();

            throw new ArgumentException($"Unknown node type [{node.GetType().Name}].", nameof(node));
        }

        /// <summary>
        /// Gets all parents of this node.
        /// </summary>
        public static IEnumerable<HtmlContainer> GetParents(this HtmlNode node)
        {
            node.GuardNotNull(nameof(node));

            while (node.Parent != null)
            {
                yield return node.Parent;
                node = node.Parent;
            }
        }

        /// <summary>
        /// Gets all element parents of this node.
        /// </summary>
        public static IEnumerable<HtmlElement> GetParentElements(this HtmlNode node)
        {
            node.GuardNotNull(nameof(node));

            return node.GetParents().OfType<HtmlElement>();
        }

        /// <summary>
        /// Gets the parent of this node as en element or null if there is no parent or it's not an element.
        /// </summary>
        public static HtmlElement GetParentElement(this HtmlNode node)
        {
            node.GuardNotNull(nameof(node));

            return node.Parent as HtmlElement;
        }

        public static IEnumerable<HtmlElement> GetNextElements(this HtmlNode node)
        {
            node.GuardNotNull(nameof(node));

            while (true)
            {
                if (node.Next == null)
                    yield break;

                if (node.Next is HtmlElement nextElement)
                    yield return nextElement;

                node = node.Next;
            }
        }

        public static HtmlElement GetNextElement(this HtmlNode node)
        {
            node.GuardNotNull(nameof(node));

            return node.GetNextElements().FirstOrDefault();
        }

        public static IEnumerable<HtmlElement> GetPreviousElements(this HtmlNode node)
        {
            node.GuardNotNull(nameof(node));

            while (true)
            {
                if (node.Previous == null)
                    yield break;

                if (node.Previous is HtmlElement previousElement)
                    yield return previousElement;

                node = node.Previous;
            }
        }

        public static HtmlElement GetPreviousElement(this HtmlNode node)
        {
            node.GuardNotNull(nameof(node));

            return node.GetPreviousElements().FirstOrDefault();
        }

        public static IEnumerable<HtmlNode> GetSiblings(this HtmlNode node)
        {
            node.GuardNotNull(nameof(node));

            foreach (var child in node.Parent.Children)
            {
                if (child != node)
                    yield return child;
            }
        }

        public static IEnumerable<HtmlElement> GetSiblingElements(this HtmlNode node)
        {
            node.GuardNotNull(nameof(node));

            return node.GetSiblings().OfType<HtmlElement>();
        }
    }
}