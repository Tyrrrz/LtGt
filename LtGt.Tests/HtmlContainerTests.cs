using System;
using System.Linq;
using LtGt.Models;
using NUnit.Framework;

namespace LtGt.Tests
{
    [TestFixture]
    public class HtmlContainerTests
    {
        [Test]
        public void GetElementById_Test()
        {
            // Arrange
            var document = HtmlParser.Default.ParseDocument(TestData.GetTestDocumentHtml());

            // Act
            var element = document.GetElementById("content");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(element, Is.Not.Null);
                Assert.That(element.GetId, Is.EqualTo("content"), "Id");
                Assert.That(element.Name, Is.EqualTo("div"), "Name");
            });
        }

        [Test]
        public void GetElementsByTagName_Test()
        {
            // Arrange
            var document = HtmlParser.Default.ParseDocument(TestData.GetTestDocumentHtml());

            // Act
            var elements = document.GetElementsByTagName("div").ToArray();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(elements, Has.Exactly(3).Items);
                Assert.That(elements.Select(e => e.Name), Has.All.EqualTo("div"), "Name");
            });
        }

        [Test]
        public void GetElementByTagName_Test()
        {
            // Arrange
            var document = HtmlParser.Default.ParseDocument(TestData.GetTestDocumentHtml());

            // Act
            var element = document.GetElementByTagName("div");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(element, Is.Not.Null);
                Assert.That(element.Name, Is.EqualTo("div"), "Name");
            });
        }

        [Test]
        public void GetElementsByClassName_Test()
        {
            // Arrange
            var document = HtmlParser.Default.ParseDocument(TestData.GetTestDocumentHtml());

            // Act
            var elements = document.GetElementsByClassName("content-child").ToArray();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(elements, Has.Exactly(2).Items);
                Assert.That(elements.Select(e => e.MatchesClassName("content-child")), Has.All.True, "Class");
                Assert.That(elements.Select(e => e.Name), Has.All.EqualTo("div"), "Name");
            });
        }

        [Test]
        public void GetElementByClassName_Test()
        {
            // Arrange
            var document = HtmlParser.Default.ParseDocument(TestData.GetTestDocumentHtml());

            // Act
            var element = document.GetElementByClassName("content-child");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(element, Is.Not.Null);
                Assert.That(element.MatchesClassName("content-child"), "Class");
                Assert.That(element.Name, Is.EqualTo("div"), "Name");
            });
        }

        [Test]
        public void GetInnerText_Test()
        {
            // Arrange
            var document = HtmlParser.Default.ParseDocument(TestData.GetTestDocumentHtml());

            // Act
            var innerText = document.GetElementById("content").GetInnerText();

            // Assert
            Assert.That(innerText, Is.EqualTo($"Text 1{Environment.NewLine}Text 2"));
        }
    }
}