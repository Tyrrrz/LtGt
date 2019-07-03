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
        /// Gets all ancestors of this <see cref="HtmlNode"/>, from immediate parent to the root node.
        /// </summary>
        public static IEnumerable<HtmlContainer> GetAncestors(this HtmlNode node)
        {
            node.GuardNotNull(nameof(node));

            while (node.Parent != null)
            {
                yield return node.Parent;
                node = node.Parent;
            }
        }

        /// <summary>
        /// Gets all siblings of this <see cref="HtmlNode"/>.
        /// </summary>
        public static IEnumerable<HtmlNode> GetSiblings(this HtmlNode node)
        {
            node.GuardNotNull(nameof(node));

            if (node.Parent == null)
                yield break;

            foreach (var child in node.Parent.Children)
            {
                if (child != node)
                    yield return child;
            }
        }

        /// <summary>
        /// Gets all siblings preceding this <see cref="HtmlNode"/>.
        /// </summary>
        public static IEnumerable<HtmlNode> GetPreviousSiblings(this HtmlNode node)
        {
            node.GuardNotNull(nameof(node));

            while (node.Previous != null)
            {
                yield return node.Previous;
                node = node.Previous;
            }
        }

        /// <summary>
        /// Gets all siblings following this <see cref="HtmlNode"/>.
        /// </summary>
        public static IEnumerable<HtmlNode> GetNextSiblings(this HtmlNode node)
        {
            node.GuardNotNull(nameof(node));

            while (node.Next != null)
            {
                yield return node.Next;
                node = node.Next;
            }
        }
    }
}