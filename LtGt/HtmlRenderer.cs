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
    public class HtmlRenderer : IHtmlRenderer
    {
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

        private void RenderComment(HtmlComment comment, StringBuilder buffer) =>
            buffer.Append("<!-- ").Append(comment.Content).Append(" -->");

        private void RenderDeclaration(HtmlDeclaration declaration, StringBuilder buffer) =>
            buffer.Append("<!").Append(declaration.Name).Append(' ').Append(declaration.Content).Append('>');

        private void RenderDocument(HtmlDocument document, StringBuilder buffer)
        {
            foreach (var node in document.Children)
            {
                RenderNode(node, buffer);
            }
        }

        private void RenderElement(HtmlElement element, StringBuilder buffer)
        {
            buffer.Append('<').Append(element.Name);

            foreach (var attribute in element.GetAttributes())
            {
                buffer.Append(' ');
                RenderAttribute(attribute, buffer);
            }

            buffer.Append('>');

            foreach (var node in element.Children.Except(element.GetAttributes()))
            {
                RenderNode(node, buffer);
            }

            buffer.Append("</").Append(element.Name).Append('>');
        }
        
        private void RenderText(HtmlText text, StringBuilder buffer) =>
            buffer.Append(WebUtility.HtmlEncode(text.Content));

        private void RenderNode(HtmlNode node, StringBuilder buffer)
        {
            switch (node)
            {
                case HtmlAttribute attribute:
                    RenderAttribute(attribute, buffer);
                    break;
                case HtmlComment comment:
                    RenderComment(comment, buffer);
                    break;
                case HtmlDeclaration declaration:
                    RenderDeclaration(declaration, buffer);
                    break;
                case HtmlDocument document:
                    RenderDocument(document, buffer);
                    break;
                case HtmlElement element:
                    RenderElement(element, buffer);
                    break;
                case HtmlText text:
                    RenderText(text, buffer);
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
}