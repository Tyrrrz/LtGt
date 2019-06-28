using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using LtGt.Models;

namespace LtGt.Internal
{
    internal class HtmlStringBuilder
    {
        private readonly StringBuilder _internalBuilder = new StringBuilder();
        private readonly Stack<HtmlElement> _parentElements = new Stack<HtmlElement>();

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
            _internalBuilder.Append(' ', _parentElements.Count * 2);
            return this;
        }

        private HtmlStringBuilder Append(HtmlDeclaration declaration) =>
            Append("<!").Append(declaration.Name).Append(' ').Append(declaration.Content).Append('>');

        private HtmlStringBuilder Append(HtmlAttribute attribute) =>
            attribute.Value != null
                ? Append(attribute.Name).Append('=').Append('"').Append(WebUtility.HtmlEncode(attribute.Value)).Append('"')
                : Append(attribute.Name);

        private HtmlStringBuilder Append(HtmlComment comment) =>
            Append("<!-- ").Append(comment.Content).Append(" -->");

        private HtmlStringBuilder Append(HtmlText text) =>
            HtmlGrammar.IsRawTextElement(_parentElements.Peek().Name)
                ? Append(text.Content)
                : Append(WebUtility.HtmlEncode(text.Content));

        private HtmlStringBuilder Append(HtmlElement element)
        {
            Append('<').Append(element.Name);

            foreach (var attribute in element.Attributes)
                Append(' ').Append(attribute);

            Append('>');

            if (HtmlGrammar.IsVoidElement(element.Name) && !element.Children.Any())
                return this;

            _parentElements.Push(element);

            foreach (var child in element.Children)
                AppendLine().Append(child);

            _parentElements.Pop();

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

        public override string ToString() => _internalBuilder.ToString();
    }
}