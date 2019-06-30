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
        public void ToXText_Test(HtmlText htmlText, XText expectedXText)
        {
            // Act
            var xText = htmlText.ToXText();

            // Assert
            Assert.That(xText, Is.EqualTo(expectedXText).Using<XNode>(XNode.EqualityComparer));
        }
    }
}