using System.Collections.Generic;
using LtGt.Models;
using NUnit.Framework;
using Tyrrrz.Extensions;

namespace LtGt.Tests
{
    [TestFixture]
    public class HtmlParserTests
    {
        private static string GetTestDocumentHtml() =>
            typeof(HtmlParserTests).Assembly.GetManifestResourceString($"{typeof(HtmlParserTests).Namespace}.Resources.TestDocument.html");

        private static IEnumerable<TestCaseData> GetTestCases_ParseDocument()
        {
            yield return new TestCaseData(
                GetTestDocumentHtml(),
                27,
                13
            ).SetName("TestDocument.html");
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_ParseDocument))]
        public void ParseDocument_Test(string source, int expectedChildNodeCount, int expectedChildElementCount)
        {
            // Act
            var document = HtmlParser.Default.ParseDocument(source);

            // Assert
            Assert.Multiple(() =>
            {
                // I know, ideally we would want to assert the entire document structure but I'm too lazy to write it out
                Assert.That(document.GetChildNodesRecursively(), Has.Exactly(expectedChildNodeCount).Items, "Child nodes");
                Assert.That(document.GetChildElementsRecursively(), Has.Exactly(expectedChildElementCount).Items, "Child elements");
            });
        }

        private static IEnumerable<TestCaseData> GetTestCases_ParseNode()
        {
            yield return new TestCaseData(
                // language=html
                "<div class=\"test-class\">test text</div>",
                new HtmlElement("div",
                    new HtmlAttribute("class", "test-class"),
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
                new HtmlElement("div",
                    new HtmlText("<test>"))
            );

            yield return new TestCaseData(
                // language=html
                "<div title=\"&lt;test title&gt;\">test text</div>",
                new HtmlElement("div",
                    new HtmlAttribute("title", "<test title>"),
                    new HtmlText("test text"))
            );

            yield return new TestCaseData(
                // language=html
                "<div><![CDATA[<test>]]></div>",
                new HtmlElement("div",
                    new HtmlText("<test>"))
            );

            yield return new TestCaseData(
                // language=html
                "<script>if (a < 5) console.log(a);</script>",
                new HtmlElement("script",
                    new HtmlText("if (a < 5) console.log(a);"))
            );

            yield return new TestCaseData(
                // language=html
                "<style>div { display: inline-block; }</style>",
                new HtmlElement("style",
                    new HtmlText("div { display: inline-block; }"))
            );

            yield return new TestCaseData(
                // language=html
                "<div >test</div>",
                new HtmlElement("div",
                    new HtmlText("test"))
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
                "<div></div>",
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
                "<div>test1<br>test2</div>",
                new HtmlElement("div",
                    new HtmlText("test1"),
                    new HtmlElement("br"),
                    new HtmlText("test2"))
            );
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_ParseNode))]
        public void ParseNode_Test(string source, HtmlNode expectedNode)
        {
            // Act
            var node = HtmlParser.Default.ParseNode(source);

            // Assert
            Assert.That(node, Is.EqualTo(expectedNode));
        }
    }
}