using System.Collections.Generic;
using System.Linq;
using LtGt.Models;
using NUnit.Framework;

namespace LtGt.Tests
{
    [TestFixture]
    public class HtmlEntityEqualityComparerTests
    {
        private static IEnumerable<TestCaseData> GetTestCases_Equals()
        {
            yield return new TestCaseData(
                null,
                null,
                true
            );

            yield return new TestCaseData(
                new HtmlAttribute("name"),
                null,
                false
            );

            yield return new TestCaseData(
                new HtmlDeclaration("name", "value"),
                new HtmlDeclaration("name", "value"),
                true
            );

            yield return new TestCaseData(
                new HtmlDeclaration("name1", "value"),
                new HtmlDeclaration("name2", "value"),
                false
            );

            yield return new TestCaseData(
                new HtmlDeclaration("name", "value1"),
                new HtmlDeclaration("name", "value2"),
                false
            );

            yield return new TestCaseData(
                new HtmlAttribute("name"),
                new HtmlAttribute("name"),
                true
            );

            yield return new TestCaseData(
                new HtmlAttribute("name1"),
                new HtmlAttribute("name2"),
                false
            );

            yield return new TestCaseData(
                new HtmlAttribute("name", "value"),
                new HtmlAttribute("name"),
                false
            );

            yield return new TestCaseData(
                new HtmlAttribute("name", "value"),
                new HtmlAttribute("name", "value"),
                true
            );

            yield return new TestCaseData(
                new HtmlAttribute("name", "value1"),
                new HtmlAttribute("name", "value2"),
                false
            );

            yield return new TestCaseData(
                new HtmlComment("test"),
                new HtmlComment("test"),
                true
            );

            yield return new TestCaseData(
                new HtmlComment("test1"),
                new HtmlComment("test2"),
                false
            );

            yield return new TestCaseData(
                new HtmlText("test"),
                new HtmlText("test"),
                true
            );

            yield return new TestCaseData(
                new HtmlText("test1"),
                new HtmlText("test2"),
                false
            );

            yield return new TestCaseData(
                new HtmlElement("div"),
                new HtmlElement("div"),
                true
            );

            yield return new TestCaseData(
                new HtmlElement("a"),
                new HtmlElement("p"),
                false
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("a", new HtmlAttribute("href", "/test")), new HtmlText("test")),
                new HtmlElement("div",
                    new HtmlElement("a", new HtmlAttribute("href", "/test")), new HtmlText("test")),
                true
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("a", new HtmlAttribute("href", "/test")), new HtmlText("test1")),
                new HtmlElement("div",
                    new HtmlElement("a", new HtmlAttribute("href", "/test")), new HtmlText("test2")),
                false
            );

            yield return new TestCaseData(
                new HtmlDocument(HtmlDeclaration.DoctypeHtml,
                    new HtmlElement("body",
                        new HtmlElement("div",
                            new HtmlText("test")))),
                new HtmlDocument(HtmlDeclaration.DoctypeHtml,
                    new HtmlElement("body",
                        new HtmlElement("div",
                            new HtmlText("test")))),
                true
            );

            yield return new TestCaseData(
                new HtmlDocument(HtmlDeclaration.DoctypeHtml,
                    new HtmlElement("body",
                        new HtmlElement("div",
                            new HtmlText("test")))),
                new HtmlDocument(HtmlDeclaration.DoctypeHtml,
                    new HtmlElement("body",
                        new HtmlElement("span",
                            new HtmlText("test")))),
                false
            );
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetHashCode()
        {
            foreach (var testCase in GetTestCases_Equals())
            {
                // Skip nulls because GetHashCode doesn't work on nulls unlike Equals
                if (testCase.Arguments.Any(a => a == null))
                    continue;

                yield return testCase;
            }
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_Equals))]
        public void Equals_Test(HtmlEntity entity1, HtmlEntity entity2, bool expected)
        {
            // Act
            var actual = HtmlEntityEqualityComparer.Default.Equals(entity1, entity2);

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetHashCode))]
        public void GetHashCode_Test(HtmlEntity entity1, HtmlEntity entity2, bool expectedEqualHashCodes)
        {
            // Act
            var hashCode1 = HtmlEntityEqualityComparer.Default.GetHashCode(entity1);
            var hashCode2 = HtmlEntityEqualityComparer.Default.GetHashCode(entity2);

            // Assert
            Assert.That(hashCode1, expectedEqualHashCodes ? Is.EqualTo(hashCode2) : Is.Not.EqualTo(hashCode2));
        }
    }
}