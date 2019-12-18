using System.Collections.Generic;
using NUnit.Framework;

namespace LtGt.Tests
{
    [TestFixture]
    public class HtmlDocumentTests
    {
        private static IEnumerable<TestCaseData> GetTestCases_GetHead()
        {
            yield return new TestCaseData(
                new HtmlDocument(new HtmlDeclaration("doctype html"), new HtmlElement("html", new HtmlElement("head"))),
                new HtmlElement("head")
            );

            yield return new TestCaseData(
                new HtmlDocument(new HtmlDeclaration("doctype html"), new HtmlElement("html")),
                null
            );
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetBody()
        {
            yield return new TestCaseData(
                new HtmlDocument(new HtmlDeclaration("doctype html"), new HtmlElement("html", new HtmlElement("body"))),
                new HtmlElement("body")
            );

            yield return new TestCaseData(
                new HtmlDocument(new HtmlDeclaration("doctype html"), new HtmlElement("html")),
                null
            );
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetTitle()
        {
            yield return new TestCaseData(
                new HtmlDocument(new HtmlDeclaration("doctype html"),
                    new HtmlElement("html",
                        new HtmlElement("head",
                            new HtmlElement("title", new HtmlText("test"))))),
                "test"
            );

            yield return new TestCaseData(
                new HtmlDocument(new HtmlDeclaration("doctype html"), new HtmlElement("html")),
                null
            );
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetHead))]
        public void GetHead_Test(HtmlDocument document, HtmlElement expected)
        {
            // Act
            var actual = document.GetHead();

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(HtmlEntityEqualityComparer.Instance));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetBody))]
        public void GetBody_Test(HtmlDocument document, HtmlElement expected)
        {
            // Act
            var actual = document.GetBody();

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(HtmlEntityEqualityComparer.Instance));
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