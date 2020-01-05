using System.Collections.Generic;
using NUnit.Framework;

namespace LtGt.Tests
{
    [TestFixture]
    public class HtmlParsingTests
    {
        private static IEnumerable<TestCaseData> GetTestCases_ParseDocument()
        {
            yield return new TestCaseData(
                // language=html
                @"<!doctype html>
<html>
    <head>
        <title>Test document</title>
        <meta charset='utf-8'>
    </head>
    <body>
        <div id='content'>
            Test
        </div>
    </body>
</html>",
                new HtmlDocument(
                    new HtmlDeclaration("doctype html"),
                    new HtmlElement("html",
                        new HtmlElement("head",
                            new HtmlElement("title", new HtmlText("Test document")),
                            new HtmlElement("meta", new HtmlAttribute("charset", "utf-8"))),
                        new HtmlElement("body",
                            new HtmlElement("div", new HtmlAttribute("id", "content"),
                                new HtmlText("Test")))))
            );
        }

        private static IEnumerable<TestCaseData> GetTestCases_ParseElement()
        {
            yield return new TestCaseData(
                // language=html
                "<div id=\"main\">test</div>",
                new HtmlElement("div", new HtmlAttribute("id", "main"),
                    new HtmlText("test"))
            );
        }

        private static IEnumerable<TestCaseData> GetTestCases_ParseNode()
        {
            yield return new TestCaseData(
                // language=html
                "<div class=\"test-class\">test text</div>",
                new HtmlElement("div", new HtmlAttribute("class", "test-class"),
                    new HtmlText("test text"))
            );

            yield return new TestCaseData(
                // language=html
                "test",
                new HtmlText("test")
            );

            yield return new TestCaseData(
                // language=html
                "<div>&lt;test&gt;</div>",
                new HtmlElement("div", new HtmlText("<test>"))
            );

            yield return new TestCaseData(
                // language=html
                "<div title=\"&lt;test title&gt;\">test text</div>",
                new HtmlElement("div", new HtmlAttribute("title", "<test title>"),
                    new HtmlText("test text"))
            );

            yield return new TestCaseData(
                // language=html
                "<div><![CDATA[<test>]]></div>",
                new HtmlElement("div", new HtmlCData("<test>"))
            );

            yield return new TestCaseData(
                // language=html
                "<script>if (a < 5) console.log(a);</script>",
                new HtmlElement("script", new HtmlText("if (a < 5) console.log(a);"))
            );

            yield return new TestCaseData(
                // language=html
                "<style>div { display: inline-block; }</style>",
                new HtmlElement("style", new HtmlText("div { display: inline-block; }"))
            );

            yield return new TestCaseData(
                // language=html
                "<div >test</div>",
                new HtmlElement("div", new HtmlText("test"))
            );

            yield return new TestCaseData(
                // language=html
                "<div attr1=\"val1\"  attr2=val2 attr3='val3' attr4>test</div>",
                new HtmlElement("div",
                    new HtmlAttribute("attr1", "val1"),
                    new HtmlAttribute("attr2", "val2"),
                    new HtmlAttribute("attr3", "val3"),
                    new HtmlAttribute("attr4"),
                    new HtmlText("test"))
            );

            yield return new TestCaseData(
                // language=html
                "<meta charset=utf-8>",
                new HtmlElement("meta", new HtmlAttribute("charset", "utf-8")));

            yield return new TestCaseData(
                // language=html
                "<div></div>",
                new HtmlElement("div")
            );

            yield return new TestCaseData(
                // language=html
                @"<div>

                  </div>",
                new HtmlElement("div")
            );

            yield return new TestCaseData(
                // language=html
                "<div />",
                new HtmlElement("div")
            );

            yield return new TestCaseData(
                // language=html
                "<div>",
                new HtmlElement("div")
            );

            yield return new TestCaseData(
                // language=html
                "<div><p>test</p>",
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlText("test")))
            );

            yield return new TestCaseData(
                // language=html
                "<div>test1<br>test2</div>",
                new HtmlElement("div",
                    new HtmlText("test1"),
                    new HtmlElement("br"),
                    new HtmlText("test2"))
            );

            yield return new TestCaseData(
                // language=html
                "     <div>test</div>",
                new HtmlElement("div",
                    new HtmlText("test"))
            );

            yield return new TestCaseData(
                // language=html
                "<x-star-rating data-attribute=\"data-value\">5</x-star-rating>",
                new HtmlElement("x-star-rating",
                    new HtmlAttribute("data-attribute", "data-value"),
                    new HtmlText("5"))
            );

            yield return new TestCaseData(
                // language=html
                "<svg width=\"108px\" height=\"17px\" viewBox=\"0 0 127 20\"><use xlink:href=\"#bandcamp-logo-color-white\"></svg>",
                new HtmlElement("svg",
                    new HtmlAttribute("width", "108px"),
                    new HtmlAttribute("height", "17px"),
                    new HtmlAttribute("viewBox", "0 0 127 20"),
                    new HtmlElement("use",
                        new HtmlAttribute("xlink:href", "#bandcamp-logo-color-white")))
                );
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_ParseDocument))]
        public void ParseDocument_Test(string source, HtmlDocument expected)
        {
            // Act
            var actual = Html.ParseDocument(source);

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(HtmlEntityEqualityComparer.Instance));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_ParseElement))]
        public void ParseElement_Test(string source, HtmlElement expected)
        {
            // Act
            var actual = Html.ParseElement(source);

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(HtmlEntityEqualityComparer.Instance));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_ParseNode))]
        public void ParseNode_Test(string source, HtmlNode expected)
        {
            // Act
            var actual = Html.ParseNode(source);

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(HtmlEntityEqualityComparer.Instance));
        }
    }
}