using System.Collections.Generic;
using System.Xml.Linq;
using LtGt.Models;
using NUnit.Framework;

namespace LtGt.Tests
{
    [TestFixture]
    public class HtmlCommentTests
    {
        private static IEnumerable<TestCaseData> GetTestCases_ToXComment()
        {
            yield return new TestCaseData(
                new HtmlComment("test test"),
                new XComment("test test")
            );
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_ToXComment))]
        public void ToXComment_Test(HtmlComment htmlComment, XComment expectedXComment)
        {
            // Act
            var xComment = htmlComment.ToXComment();

            // Assert
            Assert.That(xComment, Is.EqualTo(expectedXComment).Using<XNode>(XNode.EqualityComparer));
        }
    }
}