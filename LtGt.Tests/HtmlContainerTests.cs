using System;
using System.Collections.Generic;
using System.Linq;
using LtGt.Models;
using NUnit.Framework;

namespace LtGt.Tests
{
    [TestFixture]
    public class HtmlContainerTests
    {
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

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetElementById))]
        public void GetElementById_Test(HtmlContainer container, string id, HtmlElement expectedElement)
        {
            // Act
            var element = container.GetElementById(id);

            // Assert
            Assert.That(element, Is.EqualTo(expectedElement).Using(HtmlEntity.EqualityComparer));
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetElementsByTagName()
        {
            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("a"),
                    new HtmlElement("span",
                        new HtmlElement("p")),
                    new HtmlElement("p")),
                "*",
                new[]
                {
                    new HtmlElement("a"),
                    new HtmlElement("span",
                        new HtmlElement("p")),
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

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetElementsByTagName))]
        public void GetElementsByTagName_Test(HtmlContainer container, string tagName, IReadOnlyList<HtmlElement> expectedElements)
        {
            // Act
            var elements = container.GetElementsByTagName(tagName).ToArray();

            // Assert
            Assert.That(elements, Is.EqualTo(expectedElements).Using(HtmlEntity.EqualityComparer));
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetElementByTagName()
        {
            foreach (var testCase in GetTestCases_GetElementsByTagName())
            {
                testCase.Arguments[2] = ((IEnumerable<HtmlElement>) testCase.Arguments[2]).FirstOrDefault();

                yield return testCase;
            }
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetElementByTagName))]
        public void GetElementByTagName_Test(HtmlContainer container, string tagName, HtmlElement expectedElement)
        {
            // Act
            var element = container.GetElementByTagName(tagName);

            // Assert
            Assert.That(element, Is.EqualTo(expectedElement).Using(HtmlEntity.EqualityComparer));
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
                "test4",
                new HtmlElement[0]
            );
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetElementsByClassName))]
        public void GetElementsByClassName_Test(HtmlContainer container, string className, IReadOnlyList<HtmlElement> expectedElements)
        {
            // Act
            var elements = container.GetElementsByClassName(className).ToArray();

            // Assert
            Assert.That(elements, Is.EqualTo(expectedElements).Using(HtmlEntity.EqualityComparer));
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetElementByClassName()
        {
            foreach (var testCase in GetTestCases_GetElementsByClassName())
            {
                testCase.Arguments[2] = ((IEnumerable<HtmlElement>) testCase.Arguments[2]).FirstOrDefault();

                yield return testCase;
            }
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetElementByClassName))]
        public void GetElementByClassName_Test(HtmlContainer container, string className, HtmlElement expectedElement)
        {
            // Act
            var element = container.GetElementByClassName(className);

            // Assert
            Assert.That(element, Is.EqualTo(expectedElement).Using(HtmlEntity.EqualityComparer));
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetElementsBySelector()
        {
            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "*",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2")),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "p",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("p", new HtmlAttribute("class", "test2")),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                ".test2",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test2")),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                ".test2.test3",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "p.test2",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test2")),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "#container",
                new[]
                {
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2")))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "span#container",
                new[]
                {
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2")))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "span p",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test2"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "span > p",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test2"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "p + span",
                new[]
                {
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2")))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "p ~ p",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "#container .test2",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test2"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "span[id]",
                new[]
                {
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2")))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "span[id=\"container\"]",
                new[]
                {
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2")))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "p[class~=\"test3\"]",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "p[class^=\"test\"]",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("p", new HtmlAttribute("class", "test2")),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "p[class$=\"st2\"]",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test2"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "span[id*=\"tai\"]",
                new[]
                {
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2")))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "p:nth-child(3)",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "p:nth-last-child(3)",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test1"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "p:nth-of-type(2)",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "p:first-child",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("p", new HtmlAttribute("class", "test2"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "p:last-child",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test2")),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "p:first-of-type",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("p", new HtmlAttribute("class", "test2"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "p:last-of-type",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test2")),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "p:only-child",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test2"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "span:only-of-type",
                new[]
                {
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2")))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "p:empty",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("p", new HtmlAttribute("class", "test2")),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "img",
                new HtmlElement[0]
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                ".test4",
                new HtmlElement[0]
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                ".test2.test4",
                new HtmlElement[0]
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "#main",
                new HtmlElement[0]
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "p.test4",
                new HtmlElement[0]
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "div#container",
                new HtmlElement[0]
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "a p",
                new HtmlElement[0]
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "p[name]",
                new HtmlElement[0]
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "p[class=\"test4\"]",
                new HtmlElement[0]
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "p[class~=\"test4\"]",
                new HtmlElement[0]
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "p[class^=\"test3\"]",
                new HtmlElement[0]
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "p[class$=\"test4\"]",
                new HtmlElement[0]
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "p[class*=\"test4\"]",
                new HtmlElement[0]
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("span", new HtmlAttribute("id", "container"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                "p[class|=\"test4\"]",
                new HtmlElement[0]
            );
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetElementsBySelector))]
        public void GetElementsBySelector_Test(HtmlContainer container, string selector, IReadOnlyList<HtmlElement> expectedElements)
        {
            // Act
            var elements = container.GetElementsBySelector(selector).ToArray();

            // Assert
            Assert.That(elements, Is.EqualTo(expectedElements).Using(HtmlEntity.EqualityComparer));
        }

        private static IEnumerable<TestCaseData> GetTestCases_GetElementBySelector()
        {
            foreach (var testCase in GetTestCases_GetElementsBySelector())
            {
                testCase.Arguments[2] = ((IEnumerable<HtmlElement>) testCase.Arguments[2]).FirstOrDefault();

                yield return testCase;
            }
        }

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetElementBySelector))]
        public void GetElementBySelector_Test(HtmlContainer container, string selector, HtmlElement expectedElement)
        {
            // Act
            var element = container.GetElementBySelector(selector);

            // Assert
            Assert.That(element, Is.EqualTo(expectedElement).Using(HtmlEntity.EqualityComparer));
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

        [Test]
        [TestCaseSource(nameof(GetTestCases_GetInnerText))]
        public void GetInnerText_Test(HtmlContainer container, string expectedInnerText)
        {
            // Act
            var innerText = container.GetInnerText();

            // Assert
            Assert.That(innerText, Is.EqualTo(expectedInnerText).Using(HtmlEntity.EqualityComparer));
        }
    }
}