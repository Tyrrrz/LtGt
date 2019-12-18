using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
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
                new HtmlDeclaration("name value"),
                new HtmlDeclaration("name value"),
                true
            );

            yield return new TestCaseData(
                new HtmlDeclaration("name1 value"),
                new HtmlDeclaration("name2 value"),
                false
            );

            yield return new TestCaseData(
                new HtmlDeclaration("name value1"),
                new HtmlDeclaration("name value2"),
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
                new HtmlDocument(new HtmlDeclaration("doctype html"),
                    new HtmlElement("body",
                        new HtmlElement("div",
                            new HtmlText("test")))),
                new HtmlDocument(new HtmlDeclaration("doctype html"),
                    new HtmlElement("body",
                        new HtmlElement("div",
                            new HtmlText("test")))),
                true
            );

            yield return new TestCaseData(
                new HtmlDocument(new HtmlDeclaration("doctype html"),
                    new HtmlElement("body",
                        new HtmlElement("div",
                            new HtmlText("test")))),
                new HtmlDocument(new HtmlDeclaration("doctype html"),
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
            var actual = HtmlEntityEqualityComparer.Instance.Equals(entity1, entity2);

            // Assert
            actual.Should().Be(expected);
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetHashCode))]
        public void GetHashCode_Test(HtmlEntity entity1, HtmlEntity entity2, bool expectedEqualHashCodes)
        {
            // Act
            var hashCode1 = HtmlEntityEqualityComparer.Instance.GetHashCode(entity1);
            var hashCode2 = HtmlEntityEqualityComparer.Instance.GetHashCode(entity2);

            // Assert
            if (expectedEqualHashCodes)
                hashCode1.Should().Be(hashCode2);
            else
                hashCode1.Should().NotBe(hashCode2);
        }
    }
}