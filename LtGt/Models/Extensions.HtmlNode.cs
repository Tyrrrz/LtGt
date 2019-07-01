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
        /// Converts this <see cref="HtmlNode"/> to an instance of <see cref="XNode"/>.
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
        /// Gets all parents of this <see cref="HtmlNode"/>, from immediate parent to the root node.
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
        /// Gets all parent elements of this <see cref="HtmlNode"/>, from immediate parent to the root element.
        /// </summary>
        public static IEnumerable<HtmlElement> GetParentElements(this HtmlNode node)
        {
            node.GuardNotNull(nameof(node));

            return node.GetParents().OfType<HtmlElement>();
        }

        /// <summary>
        /// Gets the parent element of this <see cref="HtmlNode"/> or null if there is none.
        /// </summary>
        public static HtmlElement GetParentElement(this HtmlNode node)
        {
            node.GuardNotNull(nameof(node));

            return node.GetParentElements().FirstOrDefault();
        }

        /// <summary>
        /// Gets all siblings of this <see cref="HtmlNode"/>.
        /// </summary>
        public static IEnumerable<HtmlNode> GetSiblings(this HtmlNode node)
        {
            node.GuardNotNull(nameof(node));

            foreach (var child in node.Parent.Children)
            {
                if (child != node)
                    yield return child;
            }
        }

        /// <summary>
        /// Gets all sibling elements of this <see cref="HtmlNode"/>.
        /// </summary>
        public static IEnumerable<HtmlElement> GetSiblingElements(this HtmlNode node)
        {
            node.GuardNotNull(nameof(node));

            return node.GetSiblings().OfType<HtmlElement>();
        }

        /// <summary>
        /// Gets all siblings preceding this <see cref="HtmlNode"/>.
        /// </summary>
        public static IEnumerable<HtmlNode> GetPreviousSiblings(this HtmlNode node)
        {
            node.GuardNotNull(nameof(node));

            while (node.Previous != null)
            {
                yield return node;
                node = node.Previous;
            }
        }

        /// <summary>
        /// Gets all sibling elements preceding this <see cref="HtmlNode"/>.
        /// </summary>
        public static IEnumerable<HtmlElement> GetPreviousElements(this HtmlNode node)
        {
            node.GuardNotNull(nameof(node));

            return node.GetPreviousSiblings().OfType<HtmlElement>();
        }

        /// <summary>
        /// Gets the first sibling element preceding this <see cref="HtmlNode"/>.
        /// </summary>
        public static HtmlElement GetPreviousElement(this HtmlNode node)
        {
            node.GuardNotNull(nameof(node));

            return node.GetPreviousElements().FirstOrDefault();
        }

        /// <summary>
        /// Gets all siblings following this <see cref="HtmlNode"/>.
        /// </summary>
        public static IEnumerable<HtmlNode> GetNextSiblings(this HtmlNode node)
        {
            node.GuardNotNull(nameof(node));

            while (node.Next != null)
            {
                yield return node;
                node = node.Next;
            }
        }

        /// <summary>
        /// Gets all sibling elements following this <see cref="HtmlNode"/>.
        /// </summary>
        public static IEnumerable<HtmlElement> GetNextElements(this HtmlNode node)
        {
            node.GuardNotNull(nameof(node));

            return node.GetNextSiblings().OfType<HtmlElement>();
        }

        /// <summary>
        /// Gets the first sibling element following this <see cref="HtmlNode"/>.
        /// </summary>
        public static HtmlElement GetNextElement(this HtmlNode node)
        {
            node.GuardNotNull(nameof(node));

            return node.GetNextElements().FirstOrDefault();
        }
    }
}