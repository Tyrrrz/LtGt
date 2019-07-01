﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LtGt.Internal;
using LtGt.Internal.Selectors;
using Sprache;

namespace LtGt.Models
{
    public static partial class Extensions
    {
        /// <summary>
        /// Gets all direct child elements of this <see cref="HtmlContainer"/>.
        /// </summary>
        public static IEnumerable<HtmlElement> GetChildElements(this HtmlContainer container)
        {
            container.GuardNotNull(nameof(container));

            return container.Children.OfType<HtmlElement>();
        }

        /// <summary>
        /// Gets all descendants of this <see cref="HtmlContainer"/>.
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
        /// Gets all descendant elements of this <see cref="HtmlContainer"/>.
        /// </summary>
        public static IEnumerable<HtmlElement> GetDescendantElements(this HtmlContainer container)
        {
            container.GuardNotNull(nameof(container));

            return container.GetDescendants().OfType<HtmlElement>();
        }

        /// <summary>
        /// Gets the first descendant element in this <see cref="HtmlContainer"/> that has specified id or null if it's not found.
        /// Element ID comparison is case sensitive.
        /// </summary>
        public static HtmlElement GetElementById(this HtmlContainer container, string id)
        {
            container.GuardNotNull(nameof(container));
            id.GuardNotNull(nameof(id));

            return container.GetDescendantElements().FirstOrDefault(e => string.Equals(e.GetId(), id, StringComparison.Ordinal));
        }

        /// <summary>
        /// Gets all descendant elements in this <see cref="HtmlContainer"/> that have specified tag name.
        /// Element tag name comparison is not case sensitive.
        /// </summary>
        public static IEnumerable<HtmlElement> GetElementsByTagName(this HtmlContainer container, string tagName)
        {
            container.GuardNotNull(nameof(container));
            tagName.GuardNotNull(nameof(tagName));

            // Mimic JS behavior
            if (string.Equals(tagName, "*", StringComparison.OrdinalIgnoreCase))
                return container.GetDescendantElements();

            return container.GetDescendantElements().Where(e => string.Equals(e.Name, tagName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets the first descendant element in this <see cref="HtmlContainer"/> that has specified tag name or null if it's not found.
        /// Element tag name comparison is not case sensitive.
        /// </summary>
        public static HtmlElement GetElementByTagName(this HtmlContainer container, string tagName)
        {
            container.GuardNotNull(nameof(container));
            tagName.GuardNotNull(nameof(tagName));

            return container.GetElementsByTagName(tagName).FirstOrDefault();
        }

        /// <summary>
        /// Gets all descendant elements in this <see cref="HtmlContainer"/> that match specified class name.
        /// Element class name comparison is case sensitive.
        /// </summary>
        public static IEnumerable<HtmlElement> GetElementsByClassName(this HtmlContainer container, string className)
        {
            container.GuardNotNull(nameof(container));
            className.GuardNotNull(nameof(className));

            return container.GetDescendantElements().Where(e => e.MatchesClassName(className));
        }

        /// <summary>
        /// Gets the first descendant element in this <see cref="HtmlContainer"/> that matches specified class name or null if it's not found.
        /// Element class name comparison is case sensitive.
        /// </summary>
        public static HtmlElement GetElementByClassName(this HtmlContainer container, string className)
        {
            container.GuardNotNull(nameof(container));
            className.GuardNotNull(nameof(className));

            return container.GetElementsByClassName(className).FirstOrDefault();
        }

        /// <summary>
        /// Gets all descendant elements in this <see cref="HtmlContainer"/> that match specified CSS selector.
        /// See https://w3.org/TR/selectors-3 for the list of supported selectors.
        /// </summary>
        public static IEnumerable<HtmlElement> GetElementsBySelector(this HtmlContainer container, string selector)
        {
            container.GuardNotNull(nameof(container));
            selector.GuardNotNull(nameof(selector));

            // Handle 'any' selector separately for simplicity
            if (string.Equals(selector, "*", StringComparison.OrdinalIgnoreCase))
                return container.GetDescendantElements();

            var selectorParseResult = SelectorGrammar.Selector.TryParse(selector);

            // JS doesn't fail on invalid selectors so neither should we
            if (!selectorParseResult.WasSuccessful)
                return Enumerable.Empty<HtmlElement>();

            return container.GetDescendantElements().Where(e => selectorParseResult.Value.Matches(e));
        }

        /// <summary>
        /// Gets the first descendant element in this <see cref="HtmlContainer"/> that matches specified CSS selector or null if it's not found.
        /// See https://w3.org/TR/selectors-3 for the list of supported selectors.
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
                    buffer.Append(childText.Value);
                }
                else if (child is HtmlElement childElement)
                {
                    buffer.Append(childElement.GetTextRepresentation(buffer.Length == 0));
                }
            }

            return buffer.ToString();
        }

        // TODO: refactor
        /// <summary>
        /// Gets the text representation of descendants of this <see cref="HtmlContainer"/>.
        /// </summary>
        public static string GetInnerText(this HtmlContainer container)
        {
            container.GuardNotNull(nameof(container));

            return container.GetTextRepresentation();
        }
    }
}