using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace LtGt.Tests
{
    public class HtmlNodeTests
    {
        private static IEnumerable<TestCaseData> GetTestCases_GetAncestors()
        {
            HtmlNode node;

            // ReSharper disable once ObjectCreationAsStatement
            new HtmlDocument(new HtmlDeclaration("doctype html"),
                new HtmlElement("html",
                    new HtmlElement("head",
                        new HtmlElement("title", new HtmlText("Test document"))),
                    new HtmlElement("body",
                        node = new HtmlElement("div"))));

            yield return new TestCaseData(
                node,
                new HtmlContainer[]
                {
                    new HtmlElement("body",
                        new HtmlElement("div")),

                    new HtmlElement("html",
                        new HtmlElement("head",
                            new HtmlElement("title", new HtmlText("Test document"))),
                        new HtmlElement("body",
                            new HtmlElement("div"))),

                    new HtmlDocument(new HtmlDeclaration("doctype html"),
                        new HtmlElement("html",
                            new HtmlElement("head",
                                new HtmlElement("title", new HtmlText("Test document"))),
                            new HtmlElement("body",
                                new HtmlElement("div"))))
                }
            );

            // ReSharper disable once ObjectCreationAsStatement
            new HtmlElement("div",
                new HtmlElement("span",
                    node = new HtmlElement("a", new HtmlText("test"))));

            yield return new TestCaseData(
                node,
                new[]
                {
                    new HtmlElement("span",
                        new HtmlElement("a", new HtmlText("test"))),

                    new HtmlElement("div",
                        new HtmlElement("span",
                            new HtmlElement("a", new HtmlText("test"))))
                }
            );

            // ReSharper disable once ObjectCreationAsStatement
            node =
                new HtmlElement("div",
                    new HtmlElement("span",
                        new HtmlElement("a", new HtmlText("test"))));

            yield return new TestCaseData(
                node,
                new HtmlContainer[0]
            );
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetSiblings()
        {
            HtmlNode node;

            // ReSharper disable once ObjectCreationAsStatement
            new HtmlElement("div",
                new HtmlElement("a", new HtmlText("test1")),
                new HtmlElement("span",
                    new HtmlElement("p", new HtmlText("test2"))),
                node = new HtmlElement("p", new HtmlText("test3")),
                new HtmlText("test4"));

            yield return new TestCaseData(
                node,
                new HtmlNode[]
                {
                    new HtmlElement("a", new HtmlText("test1")),
                    new HtmlElement("span",
                        new HtmlElement("p", new HtmlText("test2"))),
                    new HtmlText("test4")
                }
            );

            // ReSharper disable once ObjectCreationAsStatement
            new HtmlElement("div",
                new HtmlElement("a", new HtmlText("test1")),
                new HtmlElement("span",
                    node = new HtmlElement("p", new HtmlText("test2"))),
                new HtmlElement("p", new HtmlText("test3")),
                new HtmlText("test4"));

            yield return new TestCaseData(
                node,
                new HtmlNode[0]
            );
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetPreviousSiblings()
        {
            HtmlNode node;

            // ReSharper disable once ObjectCreationAsStatement
            new HtmlElement("div",
                new HtmlElement("a", new HtmlText("test1")),
                new HtmlElement("span",
                    new HtmlElement("p", new HtmlText("test2"))),
                node = new HtmlElement("p", new HtmlText("test3")),
                new HtmlText("test4"));

            yield return new TestCaseData(
                node,
                new HtmlNode[]
                {
                    new HtmlElement("span",
                        new HtmlElement("p", new HtmlText("test2"))),
                    new HtmlElement("a", new HtmlText("test1"))
                }
            );

            // ReSharper disable once ObjectCreationAsStatement
            new HtmlElement("div",
                node = new HtmlElement("a", new HtmlText("test1")),
                new HtmlElement("span",
                    new HtmlElement("p", new HtmlText("test2"))),
                new HtmlElement("p", new HtmlText("test3")),
                new HtmlText("test4"));

            yield return new TestCaseData(
                node,
                new HtmlNode[0]
            );

            // ReSharper disable once ObjectCreationAsStatement
            new HtmlElement("div",
                new HtmlElement("a", new HtmlText("test1")),
                new HtmlElement("span",
                    node = new HtmlElement("p", new HtmlText("test2"))),
                new HtmlElement("p", new HtmlText("test3")),
                new HtmlText("test4"));

            yield return new TestCaseData(
                node,
                new HtmlNode[0]
            );
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetNextSiblings()
        {
            HtmlNode node;

            // ReSharper disable once ObjectCreationAsStatement
            new HtmlElement("div",
                new HtmlElement("a", new HtmlText("test1")),
                node = new HtmlElement("span",
                    new HtmlElement("p", new HtmlText("test2"))),
                new HtmlElement("p", new HtmlText("test3")),
                new HtmlText("test4"));

            yield return new TestCaseData(
                node,
                new HtmlNode[]
                {
                    new HtmlElement("p", new HtmlText("test3")),
                    new HtmlText("test4")
                }
            );

            // ReSharper disable once ObjectCreationAsStatement
            new HtmlElement("div",
                new HtmlElement("a", new HtmlText("test1")),
                new HtmlElement("span",
                    new HtmlElement("p", new HtmlText("test2"))),
                new HtmlElement("p", new HtmlText("test3")),
                node = new HtmlText("test4"));

            yield return new TestCaseData(
                node,
                new HtmlNode[0]
            );

            // ReSharper disable once ObjectCreationAsStatement
            new HtmlElement("div",
                new HtmlElement("a", new HtmlText("test1")),
                new HtmlElement("span",
                    node = new HtmlElement("p", new HtmlText("test2"))),
                new HtmlElement("p", new HtmlText("test3")),
                new HtmlText("test4"));

            yield return new TestCaseData(
                node,
                new HtmlNode[0]
            );
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetAncestors))]
        public void GetAncestors_Test(HtmlNode node, IReadOnlyList<HtmlContainer> expected)
        {
            // Act
            var actual = node.GetAncestors().ToArray();

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(HtmlEntityEqualityComparer.Instance));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetSiblings))]
        public void GetSiblings_Test(HtmlNode node, IReadOnlyList<HtmlNode> expected)
        {
            // Act
            var actual = node.GetSiblings().ToArray();

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(HtmlEntityEqualityComparer.Instance));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetPreviousSiblings))]
        public void GetPreviousSiblings_Test(HtmlNode node, IReadOnlyList<HtmlNode> expected)
        {
            // Act
            var actual = node.GetPreviousSiblings().ToArray();

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(HtmlEntityEqualityComparer.Instance));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetNextSiblings))]
        public void GetNextSiblings_Test(HtmlNode node, IReadOnlyList<HtmlNode> expected)
        {
            // Act
            var actual = node.GetNextSiblings().ToArray();

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(HtmlEntityEqualityComparer.Instance));
        }
    }
}