using System.Collections.Generic;
using System.Xml.Linq;
using LtGt.Models;
using NUnit.Framework;

namespace LtGt.Tests
{
    [TestFixture]
    public class HtmlTextTests
    {
        private static IEnumerable<TestCaseData> GetTestCases_ToXText()
        {
            yield return new TestCaseData(
                new HtmlText("test test"),
                new XText("test test")
            );
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_ToXText))]
        public void ToXText_Test(HtmlText text, XText expected)
        {
            // Act
            var actual = text.ToXText();

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using<XNode>(XNode.EqualityComparer));
        }
    }
}