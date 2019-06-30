using System.Collections.Generic;
using System.Xml.Linq;
using LtGt.Models;
using NUnit.Framework;

namespace LtGt.Tests
{
    public class HtmlNodeTests
    {
        private static IEnumerable<TestCaseData> GetTestCases_ToXNode()
        {
            yield return new TestCaseData(
                new HtmlAttribute("test1", "test2"),
                new XAttribute("test1", "test2")
            );

            yield return new TestCaseData(
                new HtmlComment("test"),
                new XComment("test")
            );

            yield return new TestCaseData(
                new HtmlText("test"),
                new XText("test")
            );

            yield return new TestCaseData(
                new HtmlElement("test"),
                new XElement("test")
            );

            yield return new TestCaseData(
                new HtmlDocument(HtmlDeclaration.DoctypeHtml,
                    new HtmlElement("test")),
                new XDocument(
                    new XElement("test"))
            );
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_ToXNode))]
        public void ToXNode_Test(HtmlNode htmlNode, XNode expectedXNode)
        {
            // Act
            var xNode = htmlNode.ToXNode();

            // Assert
            Assert.That(xNode, Is.EqualTo(expectedXNode).Using<XNode>(XNode.EqualityComparer));
        }
    }
}