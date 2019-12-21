using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using LtGt.Tests.Internal;
using NUnit.Framework;

namespace LtGt.Tests
{
    [TestFixture]
    public class HtmlLogicTests
    {
        private static IEnumerable<TestCaseData> GetTestCases_GetAttribute()
        {
            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlAttribute("attr1"),
                    new HtmlAttribute("attr2", "val2")),
                "attr1",
                new HtmlAttribute("attr1")
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlAttribute("attr1"),
                    new HtmlAttribute("attr2", "val2")),
                "attr2",
                new HtmlAttribute("attr2", "val2")
            );

            yield return new TestCaseData(
                new HtmlElement("div"),
                "attr1",
                null
            );
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetId()
        {
            yield return new TestCaseData(
                new HtmlElement("div", new HtmlAttribute("id", "test")),
                "test"
            );

            yield return new TestCaseData(
                new HtmlElement("div"),
                null
            );
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetClassName()
        {
            yield return new TestCaseData(
                new HtmlElement("div", new HtmlAttribute("class", "test")),
                "test"
            );

            yield return new TestCaseData(
                new HtmlElement("div", new HtmlAttribute("class", "test1 test2")),
                "test1 test2"
            );

            yield return new TestCaseData(
                new HtmlElement("div"),
                null
            );
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetClassNames()
        {
            yield return new TestCaseData(
                new HtmlElement("div", new HtmlAttribute("class", "test")),
                new[] {"test"}
            );

            yield return new TestCaseData(
                new HtmlElement("div", new HtmlAttribute("class", "test1 test2")),
                new[] {"test1", "test2"}
            );

            yield return new TestCaseData(
                new HtmlElement("div"),
                new string[0]
            );
        }

        private static IEnumerable<TestCaseData> GetTestCases_MatchesClassName()
        {
            yield return new TestCaseData(
                new HtmlElement("div", new HtmlAttribute("class", "test1")),
                "test1",
                true
            );

            yield return new TestCaseData(
                new HtmlElement("div", new HtmlAttribute("class", "test2")),
                "test2",
                true
            );

            yield return new TestCaseData(
                new HtmlElement("div", new HtmlAttribute("class", "test2")),
                "test3",
                false
            );

            yield return new TestCaseData(
                new HtmlElement("div", new HtmlAttribute("class", "test2")),
                "test1 test2",
                false
            );

            yield return new TestCaseData(
                new HtmlElement("div", new HtmlAttribute("class", "test1 test2")),
                "test1",
                true
            );

            yield return new TestCaseData(
                new HtmlElement("div", new HtmlAttribute("class", "test1 test2")),
                "test2",
                true
            );

            yield return new TestCaseData(
                new HtmlElement("div", new HtmlAttribute("class", "test1 test2")),
                "test1 test2",
                true
            );

            yield return new TestCaseData(
                new HtmlElement("div", new HtmlAttribute("class", "test2 test1")),
                "test1 test2",
                true
            );

            yield return new TestCaseData(
                new HtmlElement("div", new HtmlAttribute("class", "test3 test2 test1")),
                "test1 test2",
                true
            );
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetInnerText()
        {
            yield return new TestCaseData(
                new HtmlElement("div", new HtmlText("test")),
                "test"
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlText("test1"),
                    new HtmlText("test2")),
                "test1test2"
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlText("test1"),
                    new HtmlText("test2"),
                    new HtmlElement("br")),
                $"test1test2{Environment.NewLine}"
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlText("test1"),
                    new HtmlElement("br"),
                    new HtmlText("test2")),
                $"test1{Environment.NewLine}test2"
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlText("test1"),
                    new HtmlElement("div"),
                    new HtmlText("test2")),
                $"test1{Environment.NewLine}test2"
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("a", new HtmlText("test1")),
                    new HtmlElement("p", new HtmlText("test2")),
                    new HtmlElement("div", new HtmlElement("a", new HtmlText("test3")))),
                $"test1{Environment.NewLine}test2{Environment.NewLine}test3"
            );

            yield return new TestCaseData(
                new HtmlElement("div", new HtmlElement("span"), new HtmlElement("img")),
                ""
            );

            yield return new TestCaseData(
                new HtmlElement("div"),
                ""
            );
        }

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

        private static IEnumerable<TestCaseData> GetTestCases_GetElementById()
        {
            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                "test1",
                new HtmlElement("p", new HtmlAttribute("id", "test1"))
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                "test3",
                null
            );
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetElementsByTagName()
        {
            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("a"),
                    new HtmlElement("span",
                        new HtmlElement("p")),
                    new HtmlElement("p")),
                "p",
                new[]
                {
                    new HtmlElement("p"),
                    new HtmlElement("p")
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("a"),
                    new HtmlElement("span",
                        new HtmlElement("p")),
                    new HtmlElement("p")),
                "br",
                new HtmlElement[0]
            );
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetElementsByClassName()
        {
            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("a", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span",
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "test2",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test2")),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("a", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span",
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "test3",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("a", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span",
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "test2 test3",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("a", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span",
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "test3 test2",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("a", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span",
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "test4",
                new HtmlElement[0]
            );
        }

        private static IEnumerable<TestCaseData> GetTestCases_ToHtml()
        {
            yield return new TestCaseData(
                new HtmlDocument(new HtmlDeclaration("doctype html"),
                    new HtmlElement("html",
                        new HtmlElement("head",
                            new HtmlElement("title", new HtmlText("Test document")),
                            new HtmlElement("meta", new HtmlAttribute("name", "description"), new HtmlAttribute("content", "Test")),
                            new HtmlComment("Some test comment"),
                            new HtmlElement("script",
                                new HtmlText("let a = Math.random()*100 < 50 ? true : false;"))),
                        new HtmlElement("body",
                            new HtmlElement("div", new HtmlAttribute("id", "content"),
                                new HtmlElement("a", new HtmlAttribute("href", "https://example.com"),
                                    new HtmlText("Test <link>"))))))
            );
        }

        private static IEnumerable<TestCaseData> GetTestCases_ToXObject()
        {
            yield return new TestCaseData(
                new HtmlAttribute("name"),
                new XAttribute("name", "")
            );

            yield return new TestCaseData(
                new HtmlAttribute("name", "value"),
                new XAttribute("name", "value")
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
                new HtmlDocument(new HtmlDeclaration("doctype html"),
                    new HtmlElement("test")),
                new XDocument(
                    new XElement("test"))
            );
        }

        private static IEnumerable<TestCaseData> GetTestCases_Clone()
        {
            yield return new TestCaseData(new HtmlDeclaration("name value"));

            yield return new TestCaseData(new HtmlAttribute("name"));

            yield return new TestCaseData(new HtmlAttribute("name", "value"));

            yield return new TestCaseData(new HtmlAttribute("name"));

            yield return new TestCaseData(new HtmlComment("test"));

            yield return new TestCaseData(new HtmlText("test"));

            yield return new TestCaseData(new HtmlElement("div"));

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("a", new HtmlAttribute("href", "/test")), new HtmlText("test"))
            );

            yield return new TestCaseData(
                new HtmlDocument(new HtmlDeclaration("doctype html"),
                    new HtmlElement("body",
                        new HtmlElement("div",
                            new HtmlText("test"))))
            );
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetAttribute))]
        public void GetAttribute_Test(HtmlElement element, string attributeName, HtmlAttribute expected)
        {
            // Act
            var actual = element.GetAttribute(attributeName);

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(HtmlEntityEqualityComparer.Instance));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetId))]
        public void GetId_Test(HtmlElement element, string expected)
        {
            // Act
            var actual = element.GetId();

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetClassName))]
        public void GetClassName_Test(HtmlElement element, string expected)
        {
            // Act
            var actual = element.GetClassName();

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetClassNames))]
        public void GetClassNames_Test(HtmlElement element, IReadOnlyList<string> expected)
        {
            // Act
            var actual = element.GetClassNames();

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_MatchesClassName))]
        public void MatchesClassName_Test(HtmlElement element, string className, bool expected)
        {
            // Act
            var actual = element.ClassNameMatches(className);

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetInnerText))]
        public void GetInnerText_Test(HtmlContainer container, string expected)
        {
            // Act
            var actual = container.GetInnerText();

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
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

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetElementById))]
        public void GetElementById_Test(HtmlContainer container, string id, HtmlElement expected)
        {
            // Act
            var actual = container.GetElementById(id);

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(HtmlEntityEqualityComparer.Instance));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetElementsByTagName))]
        public void GetElementsByTagName_Test(HtmlContainer container, string tagName, IReadOnlyList<HtmlElement> expected)
        {
            // Act
            var actual = container.GetElementsByTagName(tagName).ToArray();

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(HtmlEntityEqualityComparer.Instance));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetElementsByClassName))]
        public void GetElementsByClassName_Test(HtmlContainer container, string className, IReadOnlyList<HtmlElement> expected)
        {
            // Act
            var actual = container.GetElementsByClassName(className).ToArray();

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(HtmlEntityEqualityComparer.Instance));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_ToHtml))]
        public void ToHtml_Test(HtmlNode node)
        {
            // Act
            var html = node.ToHtml();
            var roundTrip = Html.ParseNode(html);

            // Assert
            Assert.That(node, Is.EqualTo(roundTrip).Using(HtmlEntityEqualityComparer.Instance));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_ToXObject))]
        public void ToXObject_Test(HtmlEntity entity, XObject expected)
        {
            // Act
            var actual = entity.ToXObject();

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(XObjectEqualityComparer.Instance));
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_Clone))]
        public void Clone_Test(HtmlEntity entity)
        {
            // Act
            var actual = entity.Clone();

            // Assert
            Assert.That(actual, Is.Not.SameAs(entity));
            Assert.That(actual, Is.EqualTo(entity).Using(HtmlEntityEqualityComparer.Instance));
        }
    }
}