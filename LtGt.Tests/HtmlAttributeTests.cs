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
        public void ToXAttribute_Test(HtmlAttribute htmlAttribute, XAttribute expectedXAttribute)
        {
            // Act
            var xAttribute = htmlAttribute.ToXAttribute();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(xAttribute.Name, Is.EqualTo(expectedXAttribute.Name), "Name");
                Assert.That(xAttribute.Value, Is.EqualTo(expectedXAttribute.Value), "Value");
            });
        }
    }
}