using System.Linq;
using System.Net;
using System.Text;
using LtGt.Internal;
using LtGt.Models;

namespace LtGt
{
    /// <summary>
    /// HTML renderer.
    /// </summary>
    public partial class HtmlRenderer : IHtmlRenderer
    {
        private void RenderComment(HtmlComment comment, StringBuilder buffer) =>
            buffer.Append("<!-- ").Append(comment.Content).Append(" -->");

        private void RenderDeclaration(HtmlDeclaration declaration, StringBuilder buffer) =>
            buffer.Append("<!").Append(declaration.Name).Append(' ').Append(declaration.Content).Append('>');

        private void RenderText(HtmlText text, StringBuilder buffer) =>
            buffer.Append(WebUtility.HtmlEncode(text.Content));

        private void RenderAttribute(HtmlAttribute attribute, StringBuilder buffer)
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

        private void RenderElement(HtmlElement element, StringBuilder buffer)
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

            foreach (var node in nonAttributes)
            {
                RenderNode(node, buffer);
            }

            buffer.Append("</").Append(element.Name).Append('>');
        }

        private void RenderDocument(HtmlDocument document, StringBuilder buffer)
        {
            foreach (var node in document.Children)
            {
                RenderNode(node, buffer);
                buffer.AppendLine();
            }
        }

        private void RenderNode(HtmlNode node, StringBuilder buffer)
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

            var buffer = new StringBuilder();
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