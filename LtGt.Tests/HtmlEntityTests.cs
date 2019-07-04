using System.Collections.Generic;
using LtGt.Models;
using NUnit.Framework;

namespace LtGt.Tests
{
    [TestFixture]
    public class HtmlEntityTests
    {
        private static IEnumerable<TestCaseData> GetTestCases_Clone()
        {
            yield return new TestCaseData(new HtmlDeclaration("name", "value"));

            yield return new TestCaseData(new HtmlAttribute("name"));

            yield return new TestCaseData(new HtmlAttribute("name", "value"));

            yield return new TestCaseData(new HtmlAttribute("name"));

            yield return new TestCaseData(new HtmlComment("test"));

            yield return new TestCaseData(new HtmlText("test"));

            yield return new TestCaseData(new HtmlElement("div"));

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("a", new HtmlAttribute("href", "/test")), new HtmlText("test"))
            );

            yield return new TestCaseData(
                new HtmlDocument(HtmlDeclaration.DoctypeHtml,
                    new HtmlElement("body",
                        new HtmlElement("div",
                            new HtmlText("test"))))
            );
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_Clone))]
        public void Clone_Test(HtmlEntity entity)
        {
            // Act
            var actual = entity.Clone();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actual, Is.Not.SameAs(entity));
                Assert.That(actual, Is.EqualTo(entity).Using(HtmlEntity.EqualityComparer));
            });
        }
    }
}