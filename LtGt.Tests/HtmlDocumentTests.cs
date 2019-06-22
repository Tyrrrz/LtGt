using System.Collections.Generic;
using LtGt.Models;
using NUnit.Framework;

namespace LtGt.Tests
{
    [TestFixture]
    public class HtmlDocumentTests
    {
        private static IEnumerable<TestCaseData> GetTestCases_GetHead()
        {
            yield return new TestCaseData(
                new HtmlDocument(new HtmlElement("html", new HtmlElement("head"))),
                new HtmlElement("head")
            );

            yield return new TestCaseData(
                new HtmlDocument(new HtmlElement("html")),
                null
            );
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetHead))]
        public void GetHead_Test(HtmlDocument document, HtmlElement expectedElement)
        {
            // Act
            var element = document.GetHead();

            // Assert
            Assert.That(element, Is.EqualTo(expectedElement));
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetBody()
        {
            yield return new TestCaseData(
                new HtmlDocument(new HtmlElement("html", new HtmlElement("body"))),
                new HtmlElement("body")
            );

            yield return new TestCaseData(
                new HtmlDocument(new HtmlElement("html")),
                null
            );
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetBody))]
        public void GetBody_Test(HtmlDocument document, HtmlElement expectedElement)
        {
            // Act
            var element = document.GetBody();

            // Assert
            Assert.That(element, Is.EqualTo(expectedElement));
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetTitle()
        {
            yield return new TestCaseData(
                new HtmlDocument(
                    new HtmlElement("html",
                        new HtmlElement("head",
                            new HtmlElement("title",
                                new HtmlText("test"))))),
                "test"
            );

            yield return new TestCaseData(
                new HtmlDocument(new HtmlElement("html")),
                null
            );
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetTitle))]
        public void GetTitle_Test(HtmlDocument document, string expectedTitle)
        {
            // Act
            var title = document.GetTitle();

            // Assert
            Assert.That(title, Is.EqualTo(expectedTitle));
        }
    }
}