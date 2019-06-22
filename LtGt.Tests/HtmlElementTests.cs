using LtGt.Models;
using NUnit.Framework;

namespace LtGt.Tests
{
    [TestFixture]
    public class HtmlElementTests
    {
        [Test]
        public void GetHref_Test()
        {
            // Arrange
            var document = HtmlParser.Default.ParseDocument(TestData.GetTestDocumentHtml());

            // Act
            var href = document.GetElementByTagName("a").GetHref();

            // Assert
            Assert.That(href, Is.EqualTo("/test"));
        }

        [Test]
        public void GetSrc_Test()
        {
            // Arrange
            var document = HtmlParser.Default.ParseDocument(TestData.GetTestDocumentHtml());

            // Act
            var src = document.GetElementByTagName("img").GetSrc();

            // Assert
            Assert.That(src, Is.EqualTo("/image.jpg"));
        }
    }
}