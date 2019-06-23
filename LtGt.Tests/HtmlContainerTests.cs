using System;
using System.Collections.Generic;
using System.Linq;
using LtGt.Models;
using NUnit.Framework;

namespace LtGt.Tests
{
    [TestFixture]
    public class HtmlContainerTests
    {
        private static IEnumerable<TestCaseData> GetTestCases_GetElementById()
        {
            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                "test1",
                new HtmlElement("p", new HtmlAttribute("id", "test1"))
            );

            yield return new TestCaseData(
                new HtmlElement("div"),
                "test1",
                null
            );
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetElementById))]
        public void GetElementById_Test(HtmlContainer container, string id, HtmlElement expectedElement)
        {
            // Act
            var element = container.GetElementById(id);

            // Assert
            Assert.That(element, Is.EqualTo(expectedElement));
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetElementsByTagName()
        {
            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("a"),
                    new HtmlElement("p"),
                    new HtmlElement("p")),
                "p",
                new[]
                {
                    new HtmlElement("p"),
                    new HtmlElement("p")
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div"),
                "p",
                new HtmlElement[0]
            );
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetElementsByTagName))]
        public void GetElementsByTagName_Test(HtmlContainer container, string tagName, IReadOnlyList<HtmlElement> expectedElements)
        {
            // Act
            var elements = container.GetElementsByTagName(tagName).ToArray();

            // Assert
            Assert.That(elements, Is.EqualTo(expectedElements));
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetElementByTagName()
        {
            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("a"),
                    new HtmlElement("p"),
                    new HtmlElement("p")),
                "p",
                new HtmlElement("p")
            );

            yield return new TestCaseData(
                new HtmlElement("div"),
                "p",
                null
            );
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetElementByTagName))]
        public void GetElementByTagName_Test(HtmlContainer container, string tagName, HtmlElement expectedElement)
        {
            // Act
            var element = container.GetElementByTagName(tagName);

            // Assert
            Assert.That(element, Is.EqualTo(expectedElement));
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetElementsByClassName()
        {
            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("a", new HtmlAttribute("class", "test1")),
                    new HtmlElement("p", new HtmlAttribute("class", "test2")),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "test2",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test2")),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div"),
                "test2",
                new HtmlElement[0]
            );
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetElementsByClassName))]
        public void GetElementsByClassName_Test(HtmlContainer container, string className, IReadOnlyList<HtmlElement> expectedElements)
        {
            // Act
            var elements = container.GetElementsByClassName(className).ToArray();

            // Assert
            Assert.That(elements, Is.EqualTo(expectedElements));
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetElementByClassName()
        {
            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("a", new HtmlAttribute("class", "test1")),
                    new HtmlElement("p", new HtmlAttribute("class", "test2")),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "test2",
                new HtmlElement("p", new HtmlAttribute("class", "test2"))
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("a", new HtmlAttribute("class", "test1")),
                    new HtmlElement("p", new HtmlAttribute("class", "test2")),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "test3",
                new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))
            );

            yield return new TestCaseData(
                new HtmlElement("div"),
                "test2",
                null
            );
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetElementByClassName))]
        public void GetElementByClassName_Test(HtmlContainer container, string className, HtmlElement expectedElement)
        {
            // Act
            var element = container.GetElementByClassName(className);

            // Assert
            Assert.That(element, Is.EqualTo(expectedElement));
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetInnerText()
        {
            yield return new TestCaseData(
                new HtmlElement("div", new HtmlText("test")),
                "test"
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlText("test1"),
                    new HtmlText("test2")),
                "test1test2"
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlText("test1"),
                    new HtmlText("test2"),
                    new HtmlElement("br")),
                $"test1test2{Environment.NewLine}"
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlText("test1"),
                    new HtmlElement("br"),
                    new HtmlText("test2")),
                $"test1{Environment.NewLine}test2"
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlText("test1"),
                    new HtmlElement("div"),
                    new HtmlText("test2")),
                $"test1{Environment.NewLine}test2"
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("a", new HtmlText("test1")),
                    new HtmlElement("p", new HtmlText("test2")),
                    new HtmlElement("div", new HtmlElement("a", new HtmlText("test3")))),
                $"test1{Environment.NewLine}test2{Environment.NewLine}test3"
            );

            yield return new TestCaseData(
                new HtmlElement("div", new HtmlElement("span"), new HtmlElement("img")),
                ""
            );

            yield return new TestCaseData(
                new HtmlElement("div"),
                ""
            );
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetInnerText))]
        public void GetInnerText_Test(HtmlContainer container, string expectedInnerText)
        {
            // Act
            var innerText = container.GetInnerText();

            // Assert
            Assert.That(innerText, Is.EqualTo(expectedInnerText));
        }
    }
}