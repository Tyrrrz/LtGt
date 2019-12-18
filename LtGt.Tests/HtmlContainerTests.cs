using System;
using System.Collections.Generic;
using System.Linq;
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

        private static IEnumerable<TestCaseData> GetTestCases_QuerySelectorAll()
        {
            // Any

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                "*",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test2")),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))
                }
            );

            // Type

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                "p",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("p", new HtmlAttribute("id", "test2")),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                "br",
                new HtmlElement[0]
            );

            // Attribute

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                "[id]",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("p", new HtmlAttribute("id", "test2")),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                "[data]",
                new HtmlElement[0]
            );

            // Attribute value

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("class", "test"))),
                    new HtmlElement("p")),
                "[class=\"test\"]",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test")),
                    new HtmlElement("p", new HtmlAttribute("class", "test"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("class", "test"))),
                    new HtmlElement("p")),
                "[class=\"test0\"]",
                new HtmlElement[0]
            );

            // Attribute value (whitespace-separated-contains)

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1 test2")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test3"))),
                "[class~=\"test2\"]",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test1 test2")),
                    new HtmlElement("p", new HtmlAttribute("class", "test2"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1 test2")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test3"))),
                "[class~=\"test4\"]",
                new HtmlElement[0]
            );

            // Attribute value (starts with)

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1 test2")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test3"))),
                "[class^=\"test\"]",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test1 test2")),
                    new HtmlElement("p", new HtmlAttribute("class", "test2")),
                    new HtmlElement("p", new HtmlAttribute("class", "test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1 test2")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test3"))),
                "[class^=\"test4\"]",
                new HtmlElement[0]
            );

            // Attribute value (ends with)

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1 test2")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test3"))),
                "[class$=\"st2\"]",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test1 test2")),
                    new HtmlElement("p", new HtmlAttribute("class", "test2"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1 test2")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test3"))),
                "[class$=\"test1\"]",
                new HtmlElement[0]
            );

            // Attribute value (contains)

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1 test2")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test3"))),
                "[class*=\"st1\"]",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test1 test2"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1 test2")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test3"))),
                "[class*=\"toast\"]",
                new HtmlElement[0]
            );

            // Attribute value (hyphen-separated-starts-with)

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1-test2 test1-test2--test3")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("class", "test1-test4"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2-test5"))),
                "[class|=\"test1\"]",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test1-test2 test1-test2--test3")),
                    new HtmlElement("p", new HtmlAttribute("class", "test1-test4"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1-test2 test1-test2--test3")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("class", "test1-test4"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test2-test5"))),
                "[class|=\"test3\"]",
                new HtmlElement[0]
            );

            // Root

            yield return new TestCaseData(
                new HtmlDocument(new HtmlDeclaration("doctype html"),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test1")),
                        new HtmlElement("div",
                            new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                        new HtmlElement("p", new HtmlAttribute("id", "test3")))),
                ":root",
                new[]
                {
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test1")),
                        new HtmlElement("div",
                            new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                        new HtmlElement("p", new HtmlAttribute("id", "test3")))
                }
            );

            // Nth child

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":nth-child(1)",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("p", new HtmlAttribute("id", "test2"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":nth-child(odd)",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("p", new HtmlAttribute("id", "test2")),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":nth-child(2n+1)",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("p", new HtmlAttribute("id", "test2")),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":nth-child(even)",
                new[]
                {
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2")))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":nth-child(4)",
                new HtmlElement[0]
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":nth-child(4n)",
                new HtmlElement[0]
            );

            // Nth last child

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":nth-last-child(1)",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("id", "test2")),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":nth-last-child(odd)",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("p", new HtmlAttribute("id", "test2")),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":nth-last-child(2n+1)",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("p", new HtmlAttribute("id", "test2")),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":nth-last-child(even)",
                new[]
                {
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2")))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":nth-last-child(4)",
                new HtmlElement[0]
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":nth-last-child(4n)",
                new HtmlElement[0]
            );

            // Nth of type

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":nth-of-type(2)",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":nth-of-type(odd)",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test2"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":nth-of-type(2n+1)",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test2"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":nth-of-type(even)",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":nth-of-type(4)",
                new HtmlElement[0]
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":nth-of-type(4n)",
                new HtmlElement[0]
            );

            // Nth last of type

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":nth-last-of-type(2)",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("id", "test1"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":nth-last-of-type(odd)",
                new[]
                {
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test2")),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":nth-last-of-type(2n+1)",
                new[]
                {
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test2")),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":nth-last-of-type(even)",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("id", "test1"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":nth-last-of-type(4)",
                new HtmlElement[0]
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":nth-last-of-type(4n)",
                new HtmlElement[0]
            );

            // First child

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":first-child",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("p", new HtmlAttribute("id", "test2"))
                }
            );

            // Last child

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":last-child",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("id", "test2")),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))
                }
            );

            // First of type

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":first-of-type",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test2"))
                }
            );

            // Last of type

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":last-of-type",
                new[]
                {
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test2")),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))
                }
            );

            // Only child

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":only-child",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("id", "test2"))
                }
            );

            // Only of type

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":only-of-type",
                new[]
                {
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test2"))
                }
            );

            // Empty

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":empty",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("p", new HtmlAttribute("id", "test2")),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))
                }
            );

            // Id

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                "#test2",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("id", "test2"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                "#test4",
                new HtmlElement[0]
            );

            // Class name

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1 test2")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test3"))),
                ".test2",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test1 test2")),
                    new HtmlElement("p", new HtmlAttribute("class", "test2"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1 test2")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("class", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test3"))),
                ".test4",
                new HtmlElement[0]
            );

            // Not

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":not(#test1)",
                new[]
                {
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test2")),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":not(p)",
                new[]
                {
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2")))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":not(p[id])",
                new[]
                {
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2")))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":not(p[id$=\"st3\"])",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test2"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                ":not(*)",
                new HtmlElement[0]
            );

            // Descendant

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("div",
                        new HtmlElement("span",
                            new HtmlElement("p", new HtmlAttribute("id", "test3"))))),
                "div p",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("p", new HtmlAttribute("id", "test2")),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("div",
                        new HtmlElement("span",
                            new HtmlElement("p", new HtmlAttribute("id", "test3"))))),
                "tr p",
                new HtmlElement[0]
            );

            // Child

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("div",
                        new HtmlElement("span",
                            new HtmlElement("p", new HtmlAttribute("id", "test3"))))),
                "div > p",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("p", new HtmlAttribute("id", "test2"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("div",
                        new HtmlElement("span",
                            new HtmlElement("p", new HtmlAttribute("id", "test3"))))),
                "tr > p",
                new HtmlElement[0]
            );

            // Sibling

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                "div + p",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                "tr + p",
                new HtmlElement[0]
            );

            // Subsequent sibling

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("span",
                        new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test4"))),
                "div ~ p",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("id", "test4"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("span",
                        new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test4"))),
                "tr ~ p",
                new HtmlElement[0]
            );

            // Combined

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                "p[id=\"test2\"]",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("id", "test2"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                "p#test3",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1 test2")),
                    new HtmlElement("div", new HtmlAttribute("class", "test1 test3"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2 test3"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test4"))),
                "div.test1",
                new[]
                {
                    new HtmlElement("div", new HtmlAttribute("class", "test1 test3"),
                        new HtmlElement("p", new HtmlAttribute("class", "test2 test3")))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("div", new HtmlAttribute("class", "test1 test2"),
                        new HtmlElement("p", new HtmlAttribute("class", "test1 test3"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test1 test4"))),
                "p.test1:first-child",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("p", new HtmlAttribute("class", "test1 test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("div", new HtmlAttribute("class", "test1 test2"),
                        new HtmlElement("p", new HtmlAttribute("class", "test1 test3"))),
                    new HtmlElement("p", new HtmlAttribute("class", "test1 test4"))),
                "p.test1:nth-of-type(odd)",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("p", new HtmlAttribute("class", "test1 test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("div", new HtmlAttribute("class", "test1 test2"),
                        new HtmlElement("span", new HtmlAttribute("class", "test2"),
                            new HtmlElement("p", new HtmlAttribute("class", "test1 test3")))),
                    new HtmlElement("p", new HtmlAttribute("class", "test1 test4"))),
                "div.test2 p.test3",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test1 test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("div", new HtmlAttribute("class", "test1 test2"),
                        new HtmlElement("span", new HtmlAttribute("class", "test2"),
                            new HtmlElement("p", new HtmlAttribute("class", "test1 test3")))),
                    new HtmlElement("p", new HtmlAttribute("class", "test1 test4"))),
                "div > div.test2 > span > p",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test1 test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("div", new HtmlAttribute("class", "test1 test2"),
                        new HtmlElement("span", new HtmlAttribute("class", "test2"),
                            new HtmlElement("p", new HtmlAttribute("class", "test1 test3")))),
                    new HtmlElement("p", new HtmlAttribute("class", "test1 test4"))),
                "div div.test1 p.test1",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test1 test3"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("div", new HtmlAttribute("class", "test1 test2"),
                        new HtmlElement("span", new HtmlAttribute("class", "test2"),
                            new HtmlElement("p", new HtmlAttribute("class", "test1 test3")))),
                    new HtmlElement("p", new HtmlAttribute("class", "test1 test4"))),
                "p + div + p",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test1 test4"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("div", new HtmlAttribute("class", "test1 test2"),
                        new HtmlElement("span", new HtmlAttribute("class", "test2"),
                            new HtmlElement("p", new HtmlAttribute("class", "test1 test3")))),
                    new HtmlElement("p", new HtmlAttribute("class", "test1 test4"))),
                "p ~ div ~ p",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test1 test4"))
                }
            );

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "test1")),
                    new HtmlElement("div", new HtmlAttribute("class", "test1 test2"),
                        new HtmlElement("span", new HtmlAttribute("class", "test2"),
                            new HtmlElement("p", new HtmlAttribute("class", "test1 test3")))),
                    new HtmlElement("p", new HtmlAttribute("class", "test1 test4"))),
                "div span.test2 > p",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "test1 test3"))
                }
            );

            // Escaped identifiers

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("class", "foo(bar)"))),
                "p.foo\\(bar\\)",
                new[]
                {
                    new HtmlElement("p", new HtmlAttribute("class", "foo(bar)"))
                }
            );

            // Malformed

            yield return new TestCaseData(
                new HtmlElement("div",
                    new HtmlElement("p", new HtmlAttribute("id", "test1")),
                    new HtmlElement("div",
                        new HtmlElement("p", new HtmlAttribute("id", "test2"))),
                    new HtmlElement("p", new HtmlAttribute("id", "test3"))),
                "@a! asdk; $%^",
                new HtmlElement[0]
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
        [TestCaseSource(nameof(GetTestCases_QuerySelectorAll))]
        public void QuerySelectorAll_Test(HtmlContainer container, string selector, IReadOnlyList<HtmlElement> expected)
        {
            // Act
            var actual = container.QuerySelectorAll(selector).ToArray();

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Using(HtmlEntityEqualityComparer.Instance));
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
    }
}