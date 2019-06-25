using System;
using System.Linq;
using System.Net;
using LtGt.Internal;
using LtGt.Models;

namespace LtGt
{
    /// <summary>
    /// HTML renderer.
    /// </summary>
    public partial class HtmlRenderer : IHtmlRenderer
    {
        private bool IsVoidElement(HtmlElement element) =>
            !element.Children.Except(element.GetAttributes()).Any() &&
            (string.Equals(element.Name, "area", StringComparison.OrdinalIgnoreCase) ||
             string.Equals(element.Name, "base", StringComparison.OrdinalIgnoreCase) ||
             string.Equals(element.Name, "br", StringComparison.OrdinalIgnoreCase) ||
             string.Equals(element.Name, "col", StringComparison.OrdinalIgnoreCase) ||
             string.Equals(element.Name, "embed", StringComparison.OrdinalIgnoreCase) ||
             string.Equals(element.Name, "hr", StringComparison.OrdinalIgnoreCase) ||
             string.Equals(element.Name, "img", StringComparison.OrdinalIgnoreCase) ||
             string.Equals(element.Name, "input", StringComparison.OrdinalIgnoreCase) ||
             string.Equals(element.Name, "link", StringComparison.OrdinalIgnoreCase) ||
             string.Equals(element.Name, "meta", StringComparison.OrdinalIgnoreCase) ||
             string.Equals(element.Name, "param", StringComparison.OrdinalIgnoreCase) ||
             string.Equals(element.Name, "source", StringComparison.OrdinalIgnoreCase) ||
             string.Equals(element.Name, "track", StringComparison.OrdinalIgnoreCase) ||
             string.Equals(element.Name, "wbr", StringComparison.OrdinalIgnoreCase));

        private void RenderComment(HtmlComment comment, IndentedStringBuilder buffer) =>
            buffer.Append("<!-- ").Append(comment.Content).Append(" -->");

        private void RenderDeclaration(HtmlDeclaration declaration, IndentedStringBuilder buffer) =>
            buffer.Append("<!").Append(declaration.Name).Append(' ').Append(declaration.Content).Append('>');

        private void RenderText(HtmlText text, IndentedStringBuilder buffer) =>
            buffer.Append(WebUtility.HtmlEncode(text.Content));

        private void RenderAttribute(HtmlAttribute attribute, IndentedStringBuilder buffer)
        {
            if (attribute.Value != null)
            {
                buffer.Append(attribute.Name).Append('=').Append('"').Append(WebUtility.HtmlEncode(attribute.Value)).Append('"');
            }
            else
            {
                buffer.Append(attribute.Name);
            }
        }

        private void RenderElement(HtmlElement element, IndentedStringBuilder buffer)
        {
            var attributes = element.GetAttributes().ToArray();
            var nonAttributes = element.Children.Except(attributes).ToArray();

            buffer.Append('<').Append(element.Name);

            foreach (var attribute in attributes)
            {
                buffer.Append(' ');
                RenderAttribute(attribute, buffer);
            }

            buffer.Append('>');

            if (!IsVoidElement(element))
            {
                buffer.IncreaseIndent();

                foreach (var node in nonAttributes)
                {
                    buffer.AppendLine();
                    RenderNode(node, buffer);
                }

                buffer.DecreaseIndent().AppendLine().Append("</").Append(element.Name).Append('>');
            }
        }

        private void RenderDocument(HtmlDocument document, IndentedStringBuilder buffer)
        {
            foreach (var node in document.Children)
            {
                RenderNode(node, buffer);
                buffer.AppendLine();
            }
        }

        private void RenderNode(HtmlNode node, IndentedStringBuilder buffer)
        {
            switch (node)
            {
                case HtmlComment comment:
                    RenderComment(comment, buffer);
                    break;
                case HtmlDeclaration declaration:
                    RenderDeclaration(declaration, buffer);
                    break;
                case HtmlText text:
                    RenderText(text, buffer);
                    break;
                case HtmlAttribute attribute:
                    RenderAttribute(attribute, buffer);
                    break;
                case HtmlElement element:
                    RenderElement(element, buffer);
                    break;
                case HtmlDocument document:
                    RenderDocument(document, buffer);
                    break;
            }
        }

        /// <inheritdoc />
        public string RenderNode(HtmlNode node)
        {
            node.GuardNotNull(nameof(node));

            var buffer = new IndentedStringBuilder();
            RenderNode(node, buffer);

            return buffer.ToString();
        }
    }

    public partial class HtmlRenderer
    {
        /// <summary>
        /// Default instance of <see cref="HtmlRenderer"/>.
        /// </summary>
        public static IHtmlRenderer Default { get; } = new HtmlRenderer();
    }
}