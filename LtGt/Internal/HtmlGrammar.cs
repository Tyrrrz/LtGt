using System.Linq;
using LtGt.Models;
using Sprache;

namespace LtGt.Internal
{
    internal static class HtmlGrammar
    {
        public static readonly Parser<HtmlDeclaration> HtmlDeclaration =
            from open in Parse.String("<!")
            from name in Parse.LetterOrDigit.AtLeastOnce().Text()
            from value in Parse.CharExcept('>').Many().Text().Token()
            from close in Parse.Char('>')
            select new HtmlDeclaration(name, value);

        public static readonly Parser<HtmlComment> HtmlComment =
            from open in Parse.String("<!--")
            from content in Parse.AnyChar.Except(Parse.String("-->")).AtLeastOnce().Text().Token()
            from close in Parse.String("-->")
            select new HtmlComment(content);

        private static readonly Parser<HtmlAttribute> HtmlAttributeWithQuotedValue =
            from name in Parse.LetterOrDigit.Or(Parse.Char('-')).AtLeastOnce().Text()
            from eq in Parse.Char('=').Token()
            from quoteOpen in Parse.Char('"').Or(Parse.Char('\''))
            from value in Parse.CharExcept(quoteOpen).Many().Text()
            from quoteClose in Parse.Char(quoteOpen)
            select new HtmlAttribute(name, value);

        private static readonly Parser<HtmlAttribute> HtmlAttributeWithUnquotedValue =
            from name in Parse.LetterOrDigit.Or(Parse.Char('-')).AtLeastOnce().Text()
            from eq in Parse.Char('=').Token()
            from value in Parse.LetterOrDigit.Many().Text()
            select new HtmlAttribute(name, value);

        private static readonly Parser<HtmlAttribute> HtmlAttributeWithoutValue =
            from name in Parse.LetterOrDigit.Or(Parse.Char('-')).AtLeastOnce().Text()
            select new HtmlAttribute(name);

        public static readonly Parser<HtmlAttribute> HtmlAttribute =
            HtmlAttributeWithQuotedValue.Or(HtmlAttributeWithUnquotedValue).Or(HtmlAttributeWithoutValue);

        private static readonly Parser<HtmlElement> HtmlElementSelfClosing =
            from open in Parse.Char('<')
            from name in Parse.LetterOrDigit.AtLeastOnce().Text()
            from attributes in HtmlAttribute.Token().Many()
            from close in Parse.String("/>").TokenLeft()
            select new HtmlElement(name, attributes.ToArray());

        private static readonly Parser<HtmlElement> HtmlElementWithChildren =
            from ltOpen in Parse.Char('<')
            from nameOpen in Parse.LetterOrDigit.AtLeastOnce().Text()
            from attributes in HtmlAttribute.Token().Many()
            from gtOpen in Parse.Char('>').TokenLeft()
            from children in HtmlElementChild.Token().Many()
            from ltClose in Parse.String("</")
            from nameClose in Parse.String(nameOpen)
            from gtClose in Parse.Char('>').TokenLeft()
            select new HtmlElement(nameOpen, attributes.Concat(children).ToArray());

        public static readonly Parser<HtmlElement> HtmlElement = HtmlElementSelfClosing.Or(HtmlElementWithChildren);

        public static readonly Parser<HtmlText> HtmlText = Parse.CharExcept('<').AtLeastOnce().Text().Select(t => new HtmlText(t));

        private static readonly Parser<HtmlNode> HtmlElementChild = HtmlElement.Or<HtmlNode>(HtmlText).Or(HtmlComment);

        public static readonly Parser<HtmlDocument> HtmlDocument =
            from declarations in HtmlDeclaration.Token().Many()
            from children in HtmlElementChild.Token().Many()
            select new HtmlDocument(declarations.Concat(children).ToArray());

        public static readonly Parser<HtmlNode> HtmlNode = HtmlDocument.Or(HtmlElementChild).Or(HtmlDeclaration);
    }
}