using System.Collections.Generic;
using System.Xml.Linq;
using LtGt.Models;
using NUnit.Framework;

namespace LtGt.Tests
{
    [TestFixture]
    public class HtmlDocumentTests
    {
        private static IEnumerable<TestCaseData> GetTestCases_ToXDocument()
        {
            yield return new TestCaseData(
                new HtmlDocument(HtmlDeclaration.DoctypeHtml,
                    new HtmlElement("html",
                        new HtmlElement("head",
                            new HtmlElement("title", new HtmlText("Test document")),
                            new HtmlElement("meta", new HtmlAttribute("name", "description"), new HtmlAttribute("content", "test"))),
                        new HtmlElement("body",
                            new HtmlElement("div", new HtmlAttribute("id", "content"),
                                new HtmlElement("a", new HtmlAttribute("href", "https://example.com"),
                                    new HtmlText("Test link")))))),
                new XDocument(
                    new XElement("html",
                        new XElement("head",
                            new XElement("title", new XText("Test document")),
                            new XElement("meta", new XAttribute("name", "description"), new XAttribute("content", "test"))),
                        new XElement("body",
                            new XElement("div", new XAttribute("id", "content"),
                                new XElement("a", new XAttribute("href", "https://example.com"),
                                    new XText("Test link"))))))
            );
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetHead()
        {
            yield return new TestCaseData(
                new HtmlDocument(HtmlDeclaration.DoctypeHtml, new HtmlElement("html", new HtmlElement("head"))),
                new HtmlElement("head")
            );

            yield return new TestCaseData(
                new HtmlDocument(HtmlDeclaration.DoctypeHtml, new HtmlElement("html")),
                null
            );
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetBody()
        {
            yield return new TestCaseData(
                new HtmlDocument(HtmlDeclaration.DoctypeHtml, new HtmlElement("html", new HtmlElement("body"))),
                new HtmlElement("body")
            );

            yield return new TestCaseData(
                new HtmlDocument(HtmlDeclaration.DoctypeHtml, new HtmlElement("html")),
                null
            );
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetTitle()
        {
            yield return new TestCaseData(
                new HtmlDocument(HtmlDeclaration.DoctypeHtml,
                    new HtmlElement("html",
                        new HtmlElement("head",
                            new HtmlElement("title", new HtmlText("test"))))),
                "test"
            );

            yield return new TestCaseData(
                new HtmlDocument(HtmlDeclaration.DoctypeHtml, new HtmlElement("html")),
                null
            );
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_ToXDocument))]
        public void ToXDocument_Test(HtmlDocument document, XDocument expected)
        {
            // Act
            var actual = document.ToXDocument();

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using<XNode>(XNode.EqualityComparer));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetHead))]
        public void GetHead_Test(HtmlDocument document, HtmlElement expected)
        {
            // Act
            var actual = document.GetHead();

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(HtmlEntity.EqualityComparer));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetBody))]
        public void GetBody_Test(HtmlDocument document, HtmlElement expected)
        {
            // Act
            var actual = document.GetBody();

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(HtmlEntity.EqualityComparer));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetTitle))]
        public void GetTitle_Test(HtmlDocument document, string expected)
        {
            // Act
            var actual = document.GetTitle();

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}