using System.Collections.Generic;
using System.Linq;
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

        private static IEnumerable<TestCaseData> GetTestCases_GetAncestors()
        {
            HtmlNode node;

            // ReSharper disable once ObjectCreationAsStatement
            new HtmlDocument(HtmlDeclaration.DoctypeHtml,
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

                    new HtmlDocument(HtmlDeclaration.DoctypeHtml,
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

        private static IEnumerable<TestCaseData> GetTestCases_GetAncestorElements()
        {
            foreach (var testCase in GetTestCases_GetAncestors())
            {
                testCase.Arguments[1] = ((IEnumerable<HtmlContainer>) testCase.Arguments[1]).OfType<HtmlElement>().ToArray();
                yield return testCase;
            }
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetAncestorElement()
        {
            foreach (var testCase in GetTestCases_GetAncestorElements())
            {
                testCase.Arguments[1] = ((IEnumerable<HtmlElement>) testCase.Arguments[1]).FirstOrDefault();
                yield return testCase;
            }
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

        private static IEnumerable<TestCaseData> GetTestCases_GetSiblingElements()
        {
            foreach (var testCase in GetTestCases_GetSiblings())
            {
                testCase.Arguments[1] = ((IEnumerable<HtmlNode>) testCase.Arguments[1]).OfType<HtmlElement>().ToArray();
                yield return testCase;
            }
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

        private static IEnumerable<TestCaseData> GetTestCases_GetPreviousSiblingElements()
        {
            foreach (var testCase in GetTestCases_GetPreviousSiblings())
            {
                testCase.Arguments[1] = ((IEnumerable<HtmlNode>) testCase.Arguments[1]).OfType<HtmlElement>().ToArray();
                yield return testCase;
            }
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetPreviousSiblingElement()
        {
            foreach (var testCase in GetTestCases_GetPreviousSiblingElements())
            {
                testCase.Arguments[1] = ((IEnumerable<HtmlElement>) testCase.Arguments[1]).FirstOrDefault();
                yield return testCase;
            }
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

        private static IEnumerable<TestCaseData> GetTestCases_GetNextSiblingElements()
        {
            foreach (var testCase in GetTestCases_GetNextSiblings())
            {
                testCase.Arguments[1] = ((IEnumerable<HtmlNode>) testCase.Arguments[1]).OfType<HtmlElement>().ToArray();
                yield return testCase;
            }
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetNextSiblingElement()
        {
            foreach (var testCase in GetTestCases_GetNextSiblingElements())
            {
                testCase.Arguments[1] = ((IEnumerable<HtmlElement>) testCase.Arguments[1]).FirstOrDefault();
                yield return testCase;
            }
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_ToXNode))]
        public void ToXNode_Test(HtmlNode node, XNode expected)
        {
            // Act
            var actual = node.ToXNode();

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using<XNode>(XNode.EqualityComparer));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetAncestors))]
        public void GetAncestors_Test(HtmlNode node, IReadOnlyList<HtmlContainer> expected)
        {
            // Act
            var actual = node.GetAncestors().ToArray();

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(HtmlEntity.EqualityComparer));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetAncestorElements))]
        public void GetAncestorElements_Test(HtmlNode node, IReadOnlyList<HtmlElement> expected)
        {
            // Act
            var actual = node.GetAncestorElements().ToArray();

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(HtmlEntity.EqualityComparer));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetAncestorElement))]
        public void GetAncestorElement_Test(HtmlNode node, HtmlElement expected)
        {
            // Act
            var actual = node.GetAncestorElement();

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(HtmlEntity.EqualityComparer));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetSiblings))]
        public void GetSiblings_Test(HtmlNode node, IReadOnlyList<HtmlNode> expected)
        {
            // Act
            var actual = node.GetSiblings().ToArray();

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(HtmlEntity.EqualityComparer));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetSiblingElements))]
        public void GetSiblingElements_Test(HtmlNode node, IReadOnlyList<HtmlElement> expected)
        {
            // Act
            var actual = node.GetSiblingElements().ToArray();

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(HtmlEntity.EqualityComparer));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetPreviousSiblings))]
        public void GetPreviousSiblings_Test(HtmlNode node, IReadOnlyList<HtmlNode> expected)
        {
            // Act
            var actual = node.GetPreviousSiblings().ToArray();

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(HtmlEntity.EqualityComparer));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetPreviousSiblingElements))]
        public void GetPreviousSiblingElements_Test(HtmlNode node, IReadOnlyList<HtmlElement> expected)
        {
            // Act
            var actual = node.GetPreviousSiblingElements().ToArray();

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(HtmlEntity.EqualityComparer));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetPreviousSiblingElement))]
        public void GetPreviousSiblingElement_Test(HtmlNode node, HtmlElement expected)
        {
            // Act
            var actual = node.GetPreviousElement();

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(HtmlEntity.EqualityComparer));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetNextSiblings))]
        public void GetNextSiblings_Test(HtmlNode node, IReadOnlyList<HtmlNode> expected)
        {
            // Act
            var actual = node.GetNextSiblings().ToArray();

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(HtmlEntity.EqualityComparer));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetNextSiblingElements))]
        public void GetNextSiblingElements_Test(HtmlNode node, IReadOnlyList<HtmlElement> expected)
        {
            // Act
            var actual = node.GetNextSiblingElements().ToArray();

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(HtmlEntity.EqualityComparer));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetNextSiblingElement))]
        public void GetNextSiblingElement_Test(HtmlNode node, HtmlElement expected)
        {
            // Act
            var actual = node.GetNextSiblingElement();

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(HtmlEntity.EqualityComparer));
        }
    }
}