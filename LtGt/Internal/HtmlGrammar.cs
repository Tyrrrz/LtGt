using System.Linq;
using System.Net;
using LtGt.Models;
using Sprache;

namespace LtGt.Internal
{
    internal static class HtmlGrammar
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

        // Comment

        public static readonly Parser<HtmlComment> HtmlComment =
            from open in Parse.String("<!--")
            from content in Parse.AnyChar.Except(Parse.String("-->")).Many().Text().Select(WebUtility.HtmlDecode).Select(t => t.Trim())
            from close in Parse.String("-->")
            select new HtmlComment(content);

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

        // Element

        private static readonly Parser<string> HtmlElementName = Parse.LetterOrDigit.AtLeastOnce().Text();

        private static readonly Parser<HtmlElement> HtmlElementWithChildren =
            from ltOpen in Parse.Char('<')
            from nameOpen in HtmlElementName
            from attributes in HtmlAttribute.Token().Many()
            from gtOpen in Parse.Char('>').TokenLeft()
            from children in ElementChildHtmlNode.Token().Many()
            from ltClose in Parse.String("</")
            from nameClose in Parse.String(nameOpen)
            from gtClose in Parse.Char('>').TokenLeft()
            select new HtmlElement(nameOpen, attributes.Concat(children).ToArray());

        private static readonly Parser<HtmlElement> SelfClosingHtmlElement =
            from open in Parse.Char('<')
            from name in HtmlElementName
            from attributes in HtmlAttribute.Token().Many()
            from close in Parse.String("/>").Or(Parse.String(">")).TokenLeft()
            select new HtmlElement(name, attributes.ToArray<HtmlNode>());

        public static readonly Parser<HtmlElement> HtmlElement = HtmlElementWithChildren.Or(SelfClosingHtmlElement);

        // Text

        private static readonly Parser<HtmlText> CDataHtmlText =
            from open in Parse.String("<![CDATA[")
            from content in Parse.AnyChar.Except(Parse.String("]]>")).Many().Text()
            from close in Parse.String("]]>")
            select new HtmlText(content);

        public static readonly Parser<HtmlText> RegularHtmlText =
            Parse.CharExcept('<').AtLeastOnce().Text().Select(WebUtility.HtmlDecode).Select(t => t.Trim()).Select(t => new HtmlText(t));


        public static readonly Parser<HtmlText> HtmlText = CDataHtmlText.Or(RegularHtmlText);

        // Document

        public static readonly Parser<HtmlDocument> HtmlDocument =
            from declarations in HtmlDeclaration.Token().AtLeastOnce()
            from children in HtmlNode.Token().Many()
            select new HtmlDocument(declarations.Concat(children).ToArray());

        // Node

        private static readonly Parser<HtmlNode> ElementChildHtmlNode = HtmlElement.Or<HtmlNode>(HtmlText).Or(HtmlComment);

        public static readonly Parser<HtmlNode> HtmlNode = HtmlDocument.Or(ElementChildHtmlNode).Or(HtmlDeclaration);
    }
}