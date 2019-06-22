using LtGt.Models;
using NUnit.Framework;

namespace LtGt.Tests
{
    [TestFixture]
    public class HtmlParserTests
    {
        [Test]
        public void LoadDocument_Test()
        {
            // Arrange
            var source = TestData.GetTestDocumentHtml();

            // Act
            var document = HtmlParser.Default.ParseDocument(source);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(document.GetChildNodesRecursively(), Has.Exactly(30).Items, "Child nodes");
                Assert.That(document.GetChildElementsRecursively(), Has.Exactly(13).Items, "Child elements");
            });
        }
    }
}