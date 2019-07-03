using System.Collections.Generic;
using System.Xml.Linq;
using LtGt.Models;
using NUnit.Framework;

namespace LtGt.Tests
{
    [TestFixture]
    public class HtmlAttributeTests
    {
        private static IEnumerable<TestCaseData> GetTestCases_ToXAttribute()
        {
            yield return new TestCaseData(
                new HtmlAttribute("name"),
                new XAttribute("name", "")
            );

            yield return new TestCaseData(
                new HtmlAttribute("name", "value"),
                new XAttribute("name", "value")
            );
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_ToXAttribute))]
        public void ToXAttribute_Test(HtmlAttribute attribute, XAttribute expected)
        {
            // Act
            var actual = attribute.ToXAttribute();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actual.Name, Is.EqualTo(expected.Name), "Name");
                Assert.That(actual.Value, Is.EqualTo(expected.Value), "Value");
            });
        }
    }
}