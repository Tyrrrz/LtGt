using System.Collections.Generic;
using System.Linq;
using LtGt.Models;
using NUnit.Framework;

namespace LtGt.Tests
{
    [TestFixture]
    public class HtmlElementTests
    {
        private static IEnumerable<TestCaseData> GetTestCases_GetAttributes()
        {
            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlAttribute("attr1"),
                    new HtmlAttribute("attr2", "val2")),
                new[]
                {
                    new HtmlAttribute("attr1"),
                    new HtmlAttribute("attr2", "val2")
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div"),
                new HtmlAttribute[0]
            );
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetAttributes))]
        public void GetAttributes_Test(HtmlElement element, IReadOnlyList<HtmlAttribute> expectedAttributes)
        {
            // Act
            var attributes = element.GetAttributes().ToArray();

            // Assert
            Assert.That(attributes, Is.EqualTo(expectedAttributes));
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

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetAttribute))]
        public void GetAttribute_Test(HtmlElement element, string attributeName, HtmlAttribute expectedAttribute)
        {
            // Act
            var attribute = element.GetAttribute(attributeName);

            // Assert
            Assert.That(attribute, Is.EqualTo(expectedAttribute));
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

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetId))]
        public void GetId_Test(HtmlElement element, string expectedId)
        {
            // Act
            var id = element.GetId();

            // Assert
            Assert.That(id, Is.EqualTo(expectedId));
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

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetClassName))]
        public void GetClassName_Test(HtmlElement element, string expectedClassName)
        {
            // Act
            var className = element.GetClassName();

            // Assert
            Assert.That(className, Is.EqualTo(expectedClassName));
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetClassList()
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

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetClassList))]
        public void GetClassList_Test(HtmlElement element, IReadOnlyList<string> expectedClassList)
        {
            // Act
            var classList = element.GetClassList();

            // Assert
            Assert.That(classList, Is.EqualTo(expectedClassList));
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

        [Test]
        [TestCaseSource(nameof(GetTestCases_MatchesClassName))]
        public void MatchesClassName_Test(HtmlElement element, string className, bool expectedMatches)
        {
            // Act
            var matches = element.MatchesClassName(className);

            // Assert
            Assert.That(matches, Is.EqualTo(expectedMatches));
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

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetHref))]
        public void GetHref_Test(HtmlElement element, string expectedHref)
        {
            // Act
            var href = element.GetHref();

            // Assert
            Assert.That(href, Is.EqualTo(expectedHref));
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
        [TestCaseSource(nameof(GetTestCases_GetSrc))]
        public void GetSrc_Test(HtmlElement element, string expectedSrc)
        {
            // Act
            var src = element.GetSrc();

            // Assert
            Assert.That(src, Is.EqualTo(expectedSrc));
        }
    }
}