using System.Collections.Generic;
using System.Xml.Linq;
using LtGt.Models;
using NUnit.Framework;

namespace LtGt.Tests
{
    [TestFixture]
    public class HtmlElementTests
    {
        private static IEnumerable<TestCaseData> GetTestCases_ToXElement()
        {
            yield return new TestCaseData(
                new HtmlElement("div"),
                new XElement("div")
            );

            yield return new TestCaseData(
                new HtmlElement("div", new HtmlAttribute("id", "test")),
                new XElement("div", new XAttribute("id", "test"))
            );

            yield return new TestCaseData(
                new HtmlElement("div", new HtmlAttribute("id", "test1"),
                    new HtmlElement("p", new HtmlText("test2")),
                    new HtmlText("test3")),
                new XElement("div", new XAttribute("id", "test1"),
                    new XElement("p", new XText("test2")),
                    new XText("test3"))
            );
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetAttribute()
        {
            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlAttribute("attr1"),
                    new HtmlAttribute("attr2", "val2")),
                "attr1",
                new HtmlAttribute("attr1")
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlAttribute("attr1"),
                    new HtmlAttribute("attr2", "val2")),
                "attr2",
                new HtmlAttribute("attr2", "val2")
            );

            yield return new TestCaseData(
                new HtmlElement("div"),
                "attr1",
                null
            );
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetId()
        {
            yield return new TestCaseData(
                new HtmlElement("div", new HtmlAttribute("id", "test")),
                "test"
            );

            yield return new TestCaseData(
                new HtmlElement("div"),
                null
            );
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetClassName()
        {
            yield return new TestCaseData(
                new HtmlElement("div", new HtmlAttribute("class", "test")),
                "test"
            );

            yield return new TestCaseData(
                new HtmlElement("div", new HtmlAttribute("class", "test1 test2")),
                "test1 test2"
            );

            yield return new TestCaseData(
                new HtmlElement("div"),
                null
            );
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetClassNames()
        {
            yield return new TestCaseData(
                new HtmlElement("div", new HtmlAttribute("class", "test")),
                new[] {"test"}
            );

            yield return new TestCaseData(
                new HtmlElement("div", new HtmlAttribute("class", "test1 test2")),
                new[] {"test1", "test2"}
            );

            yield return new TestCaseData(
                new HtmlElement("div"),
                new string[0]
            );
        }

        private static IEnumerable<TestCaseData> GetTestCases_MatchesClassName()
        {
            yield return new TestCaseData(
                new HtmlElement("div", new HtmlAttribute("class", "test1")),
                "test1",
                true
            );

            yield return new TestCaseData(
                new HtmlElement("div", new HtmlAttribute("class", "test2")),
                "test2",
                true
            );

            yield return new TestCaseData(
                new HtmlElement("div", new HtmlAttribute("class", "test2")),
                "test3",
                false
            );

            yield return new TestCaseData(
                new HtmlElement("div", new HtmlAttribute("class", "test2")),
                "test1 test2",
                false
            );

            yield return new TestCaseData(
                new HtmlElement("div", new HtmlAttribute("class", "test1 test2")),
                "test1",
                true
            );

            yield return new TestCaseData(
                new HtmlElement("div", new HtmlAttribute("class", "test1 test2")),
                "test2",
                true
            );

            yield return new TestCaseData(
                new HtmlElement("div", new HtmlAttribute("class", "test1 test2")),
                "test1 test2",
                true
            );

            yield return new TestCaseData(
                new HtmlElement("div", new HtmlAttribute("class", "test2 test1")),
                "test1 test2",
                true
            );

            yield return new TestCaseData(
                new HtmlElement("div", new HtmlAttribute("class", "test3 test2 test1")),
                "test1 test2",
                true
            );
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetHref()
        {
            yield return new TestCaseData(
                new HtmlElement("a", new HtmlAttribute("href", "test")),
                "test"
            );

            yield return new TestCaseData(
                new HtmlElement("a"),
                null
            );
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetSrc()
        {
            yield return new TestCaseData(
                new HtmlElement("img", new HtmlAttribute("src", "test")),
                "test"
            );

            yield return new TestCaseData(
                new HtmlElement("img"),
                null
            );
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_ToXElement))]
        public void ToXElement_Test(HtmlElement element, XElement expected)
        {
            // Act
            var actual = element.ToXElement();

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using<XNode>(XNode.EqualityComparer));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetAttribute))]
        public void GetAttribute_Test(HtmlElement element, string attributeName, HtmlAttribute expected)
        {
            // Act
            var actual = element.GetAttribute(attributeName);

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(HtmlEntity.EqualityComparer));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetId))]
        public void GetId_Test(HtmlElement element, string expected)
        {
            // Act
            var actual = element.GetId();

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetClassName))]
        public void GetClassName_Test(HtmlElement element, string expected)
        {
            // Act
            var actual = element.GetClassName();

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetClassNames))]
        public void GetClassNames_Test(HtmlElement element, IReadOnlyList<string> expected)
        {
            // Act
            var actual = element.GetClassNames();

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_MatchesClassName))]
        public void MatchesClassName_Test(HtmlElement element, string className, bool expected)
        {
            // Act
            var actual = element.MatchesClassName(className);

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetHref))]
        public void GetHref_Test(HtmlElement element, string expected)
        {
            // Act
            var actual = element.GetHref();

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetSrc))]
        public void GetSrc_Test(HtmlElement element, string expected)
        {
            // Act
            var actual = element.GetSrc();

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}