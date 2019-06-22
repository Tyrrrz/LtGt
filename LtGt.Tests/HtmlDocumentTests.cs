using LtGt.Models;
using NUnit.Framework;

namespace LtGt.Tests
{
    [TestFixture]
    public class HtmlDocumentTests
    {
        [Test]
        public void GetHead_Test()
        {
            // Arrange
            var document = HtmlParser.Default.ParseDocument(TestData.GetTestDocumentHtml());

            // Act
            var head = document.GetHead();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(head, Is.Not.Null);
                Assert.That(head.Name, Is.EqualTo("head"), "Name");
            });
        }

        [Test]
        public void GetBody_Test()
        {
            // Arrange
            var document = HtmlParser.Default.ParseDocument(TestData.GetTestDocumentHtml());

            // Act
            var body = document.GetBody();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(body, Is.Not.Null);
                Assert.That(body.Name, Is.EqualTo("body"), "Name");
            });
        }

        [Test]
        public void GetTitle_Test()
        {
            // Arrange
            var document = HtmlParser.Default.ParseDocument(TestData.GetTestDocumentHtml());

            // Act
            var title = document.GetTitle();

            // Assert
            Assert.That(title, Is.EqualTo("Test document"));
        }
    }
}