using System.Collections.Generic;
using LtGt.Models;
using NUnit.Framework;
using Tyrrrz.Extensions;

namespace LtGt.Tests
{
    [TestFixture]
    public class HtmlParserTests
    {
        private static string GetTestDocumentHtml() =>
            typeof(HtmlParserTests).Assembly.GetManifestResourceString($"{typeof(HtmlParserTests).Namespace}.Resources.TestDocument.html");

        private static IEnumerable<TestCaseData> GetTestCases_ParseDocument()
        {
            yield return new TestCaseData(
                GetTestDocumentHtml(),
                27,
                13
            ).SetName("TestDocument.html");
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_ParseDocument))]
        public void ParseDocument_Test(string source, int expectedChildNodeCount, int expectedChildElementCount)
        {
            // Act
            var document = HtmlParser.Default.ParseDocument(source);

            // Assert
            Assert.Multiple(() =>
            {
                // I know, ideally we would want to assert the entire document structure but I'm too lazy to write it out
                Assert.That(document.GetChildNodesRecursively(), Has.Exactly(expectedChildNodeCount).Items, "Child nodes");
                Assert.That(document.GetChildElementsRecursively(), Has.Exactly(expectedChildElementCount).Items, "Child elements");
            });
        }
    }
}