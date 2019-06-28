using System;
using System.Linq;
using System.Net;
using LtGt.Models;
using Sprache;

namespace LtGt.Internal
{
    internal static partial class HtmlGrammar
    {
        public static bool IsVoidElement(string elementName) =>
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

        public static bool IsRawTextElement(string elementName) =>
            string.Equals(elementName, "script", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(elementName, "style", StringComparison.OrdinalIgnoreCase);
    }

    internal static partial class HtmlGrammar
    {
        private static Parser<T> TokenLeft<T>(this Parser<T> parser) => i =>
        {
            while (char.IsWhiteSpace(i.Current))
                i = i.Advance();

            return parser(i);
        };

        // Declaration

        public static readonly Parser<HtmlDeclaration> HtmlDeclaration =
            from open in Parse.String("<!")
            from name in Parse.LetterOrDigit.AtLeastOnce().Text()
            from value in Parse.CharExcept('>').Many().Text().Select(t => t.Trim())
            from close in Parse.Char('>')
            select new HtmlDeclaration(name, value);

        // Attribute

        private static readonly Parser<string> HtmlAttributeName =
            Parse.CharExcept(c => char.IsControl(c) || char.IsWhiteSpace(c) || c == '\'' || c == '"' || c == '=' || c == '>' || c == '/',
                    "Invalid character for attribute name")
                .AtLeastOnce().Text();

        private static readonly Parser<HtmlAttribute> QuotedHtmlAttribute =
            from name in HtmlAttributeName
            from eq in Parse.Char('=').Token()
            from quoteOpen in Parse.Char('"').Or(Parse.Char('\''))
            from value in Parse.CharExcept(quoteOpen).Many().Text().Select(WebUtility.HtmlDecode)
            from quoteClose in Parse.Char(quoteOpen)
            select new HtmlAttribute(name, value);

        private static readonly Parser<HtmlAttribute> UnquotedHtmlAttribute =
            from name in HtmlAttributeName
            from eq in Parse.Char('=').Token()
            from value in Parse.LetterOrDigit.Many().Text().Select(WebUtility.HtmlDecode)
            select new HtmlAttribute(name, value);

        private static readonly Parser<HtmlAttribute> ValuelessHtmlAttribute = HtmlAttributeName.Select(n => new HtmlAttribute(n));

        public static readonly Parser<HtmlAttribute> HtmlAttribute =
            QuotedHtmlAttribute.Or(UnquotedHtmlAttribute).Or(ValuelessHtmlAttribute);

        // Comment

        public static readonly Parser<HtmlComment> HtmlComment =
            from open in Parse.String("<!--")
            from content in Parse.AnyChar.Except(Parse.String("-->")).Many().Text().Select(WebUtility.HtmlDecode).Select(t => t.Trim())
            from close in Parse.String("-->")
            select new HtmlComment(content);

        // Text

        private static readonly Parser<HtmlText> CDataHtmlText =
            from open in Parse.String("<![CDATA[")
            from content in Parse.AnyChar.Except(Parse.String("]]>")).Many().Text().Select(t => t.Trim())
            from close in Parse.String("]]>")
            select new HtmlText(content);

        public static readonly Parser<HtmlText> NormalHtmlText =
            Parse.CharExcept('<').AtLeastOnce().Text().Select(WebUtility.HtmlDecode).Select(t => t.Trim()).Select(t => new HtmlText(t));


        public static readonly Parser<HtmlText> HtmlText = CDataHtmlText.Or(NormalHtmlText);

        // Element

        private static readonly Parser<string> HtmlElementName = Parse.LetterOrDigit.AtLeastOnce().Text();

        private static readonly Parser<HtmlElement> RawTextHtmlElement =
            from ltOpen in Parse.Char('<')
            from nameOpen in HtmlElementName.Where(IsRawTextElement)
            from attributes in HtmlAttribute.Token().Many()
            from gtOpen in Parse.Char('>').TokenLeft()
            from text in Parse.AnyChar.Except(Parse.String($"</{nameOpen}")).Many().Text().Select(t => t.Trim())
            from ltClose in Parse.String("</").TokenLeft()
            from nameClose in Parse.String(nameOpen)
            from gtClose in Parse.Char('>').TokenLeft()
            select new HtmlElement(nameOpen, attributes.ToArray(), new[] {new HtmlText(text)});

        private static readonly Parser<HtmlElement> VoidHtmlElement =
            from open in Parse.Char('<')
            from name in HtmlElementName.Where(IsVoidElement)
            from attributes in HtmlAttribute.Token().Many()
            from close in Parse.Char('>').TokenLeft()
            select new HtmlElement(name, attributes.ToArray());

        private static readonly Parser<HtmlElement> SelfClosingHtmlElement =
            from open in Parse.Char('<')
            from name in HtmlElementName
            from attributes in HtmlAttribute.Token().Many()
            from close in Parse.String("/>").TokenLeft()
            select new HtmlElement(name, attributes.ToArray());

        private static readonly Parser<HtmlElement> NormalHtmlElement =
            from ltOpen in Parse.Char('<')
            from nameOpen in HtmlElementName
            from attributes in HtmlAttribute.Token().Many()
            from gtOpen in Parse.Char('>').TokenLeft()
            from children in ElementChildHtmlNode.Token().Many()
            from ltClose in Parse.String("</").TokenLeft()
            from nameClose in Parse.String(nameOpen)
            from gtClose in Parse.Char('>').TokenLeft()
            select new HtmlElement(nameOpen, attributes.ToArray(), children.ToArray());

        public static readonly Parser<HtmlElement> HtmlElement =
            RawTextHtmlElement.Or(VoidHtmlElement).Or(SelfClosingHtmlElement).Or(NormalHtmlElement);

        // Document

        public static readonly Parser<HtmlDocument> HtmlDocument =
            from declaration in HtmlDeclaration.Token()
            from children in ElementChildHtmlNode.Token().Many()
            select new HtmlDocument(declaration, children.ToArray());

        // Node

        private static readonly Parser<HtmlNode> ElementChildHtmlNode = HtmlElement.Or<HtmlNode>(HtmlComment).Or(HtmlText);

        public static readonly Parser<HtmlNode> HtmlNode = HtmlDocument.Or(ElementChildHtmlNode);
    }
}