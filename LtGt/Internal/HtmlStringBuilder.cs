using System;
using System.Linq;
using System.Net;
using System.Text;
using LtGt.Models;

namespace LtGt.Internal
{
    internal class HtmlStringBuilder
    {
        private static bool IsVoidElementName(string? elementName) =>
            string.Equals(elementName, "area", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(elementName, "base", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(elementName, "br", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(elementName, "col", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(elementName, "embed", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(elementName, "hr", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(elementName, "img", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(elementName, "input", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(elementName, "link", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(elementName, "meta", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(elementName, "param", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(elementName, "source", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(elementName, "track", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(elementName, "wbr", StringComparison.OrdinalIgnoreCase);

        private static bool IsRawTextElementName(string? elementName) =>
            string.Equals(elementName, "script", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(elementName, "style", StringComparison.OrdinalIgnoreCase);

        private readonly StringBuilder _internalBuilder = new StringBuilder();

        private int _depth;

        private HtmlStringBuilder Append(string value)
        {
            _internalBuilder.Append(value);
            return this;
        }

        private HtmlStringBuilder Append(char value)
        {
            _internalBuilder.Append(value);
            return this;
        }

        private HtmlStringBuilder AppendLine()
        {
            _internalBuilder.AppendLine();
            _internalBuilder.Append(' ', _depth * 2);
            return this;
        }

        private HtmlStringBuilder Append(HtmlDeclaration declaration) =>
            Append("<!").Append(declaration.Name).Append(' ').Append(declaration.Value).Append('>');

        private HtmlStringBuilder Append(HtmlAttribute attribute) =>
            attribute.Value != null
                ? Append(attribute.Name).Append('=').Append('"').Append(WebUtility.HtmlEncode(attribute.Value)).Append('"')
                : Append(attribute.Name);

        private HtmlStringBuilder Append(HtmlComment comment) =>
            Append("<!-- ").Append(comment.Value).Append(" -->");

        private HtmlStringBuilder Append(HtmlText text) =>
            IsRawTextElementName((text.Parent as HtmlElement)?.Name)
                ? Append(text.Value)
                : Append(WebUtility.HtmlEncode(text.Value));

        private HtmlStringBuilder Append(HtmlElement element)
        {
            Append('<').Append(element.Name);

            foreach (var attribute in element.Attributes)
                Append(' ').Append(attribute);

            Append('>');

            if (IsVoidElementName(element.Name) && !element.Children.Any())
                return this;

            _depth++;

            foreach (var child in element.Children)
                AppendLine().Append(child);

            _depth--;

            AppendLine().Append("</").Append(element.Name).Append('>');

            return this;
        }

        private HtmlStringBuilder Append(HtmlDocument document)
        {
            Append(document.Declaration).AppendLine();

            foreach (var child in document.Children)
                Append(child).AppendLine();

            return this;
        }

        public HtmlStringBuilder Append(HtmlNode node)
        {
            if (node is HtmlComment comment)
                return Append(comment);

            if (node is HtmlText text)
                return Append(text);

            if (node is HtmlElement element)
                return Append(element);

            if (node is HtmlDocument document)
                return Append(document);

            throw new ArgumentException($"Unknown node type [{node.GetType().Name}].", nameof(node));
        }

        public override string ToString() => _internalBuilder.ToString().Trim();
    }
}