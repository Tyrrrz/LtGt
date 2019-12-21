# LtGt

[![Build](https://github.com/Tyrrrz/LtGt/workflows/CI/badge.svg?branch=master)](https://github.com/Tyrrrz/LtGt/actions)
[![Coverage](https://codecov.io/gh/Tyrrrz/LtGt/branch/master/graph/badge.svg)](https://codecov.io/gh/Tyrrrz/LtGt)
[![Version](https://img.shields.io/nuget/v/LtGt.svg)](https://nuget.org/packages/LtGt)
[![Downloads](https://img.shields.io/nuget/dt/LtGt.svg)](https://nuget.org/packages/LtGt)
[![Donate](https://img.shields.io/badge/donate-$$$-purple.svg)](https://tyrrrz.me/donate)

LtGt is a minimalistic library for working with HTML. It can parse any HTML5-compliant code into an object model which you can use to traverse nodes or locate specific elements. The library establishes itself as a foundation that you can build upon, and comes with a lot of extension methods that can help navigate the DOM easily. It also supports HTML rendering, so you can turn any HTML object tree to code.

_Currently, the object model in LtGt is immutable so it cannot be used to manipulate DOM directly._

## Download

- [NuGet](https://nuget.org/packages/LtGt): `dotnet add package LtGt`

## Features

- Parse any HTML5-compliant code
- Traverse over the DOM using LINQ or Seq
- Basic element selectors, i.e. `GetElementById()`, `GetElementsByTagName()`, etc
- CSS selectors, i.e. `div#main`, `p.info:nth-child(2n + 1)`, etc
- Convert HTML to equivalent Linq2Xml representation, i.e. `XNode`, `XElement`, etc
- Convert HTML to code
- Targets .NET Framework 4.5+ and .NET Standard 1.6+

## Screenshots

![dom](.screenshots/dom.png)
![css selectors](.screenshots/css-selectors.png)

## Usage

LtGt is an F# library but it has separate APIs for convenient usage with both F# and C#.

### Parse a document

```c#
using LtGt;

const string html = @"<!doctype html>
<html>
  <head>
    <title>Document</title>
  </head>
  <body>
    <div>Content</div>
  </body>
</html>";

var document = Html.ParseDocument(html);
```

```f#
open LtGt

let html = "<!doctype html>
<html>
  <head>
    <title>Document</title>
  </head>
  <body>
    <div>Content</div>
  </body>
</html>"

let document = Html.parseDocument html
```

### Parse a fragment

```c#
const string html = "<div id=\"some-element\"><a href=\"https://example.com\">Link</a></div>";

// Parse an element node
var element = Html.ParseElement(html);

// Parse any node
var node = Html.ParseNode(html);
```

```f#
let html = "<div id=\"some-element\"><a href=\"https://example.com\">Link</a></div>"

// Parse an element node
let element = Html.parseElement html

// Parse any node
let node = Html.parseNode html
```

### Find specific element

```c#
var element1 = document.GetElementById("menu-bar");
var element2 = document.GetElementsByTagName("div").FirstOrDefault();
var element3 = document.GetElementsByClassName("floating-button floating-button--enabled").FirstOrDefault();

var element1Data = element1.GetAttributeValue("data");
var element2Id = element2.GetId();
var element3Text = element3.GetInnerText();
```

```f#
let element1 = document |> tryElementById "menu-bar"
let element2 = document |> elementsByTagName "div" |> Seq.tryHead
let element3 = document |> elementsByClassName "floating-button floating-button--enabled" |> Seq.tryHead

let element1Data = element1 |> Option.bind (tryAttributeValue "data")
let element2Id = element2 |> Option.bind tryId
let element3Text = element3 |> Option.map innerText
```

You can leverage the full power of CSS selectors as well.

```c#
var element = document.QueryElements("div#main > span.container:empty").FirstOrDefault();
```

```f#
let element = document |> queryElements "div#main > span.container:empty" |> Seq.tryHead
```

### Check equality

You can compare two HTML entities by value, including their descendants.

```c#
var element1 = new HtmlElement("span",
    new HtmlAttribute("id", "foo"),
    new HtmlText("bar"))

var element2 = new HtmlElement("span",
    new HtmlAttribute("id", "foo"),
    new HtmlText("bar"))

var element3 = new HtmlElement("span",
    new HtmlAttribute("id", "foo"),
    new HtmlText("oof"))

var firstTwoEqual = HtmlEntityEqualityComparer.Instance.Equals(element1, element2); // true
var lastTwoEqual = HtmlEntityEqualityComparer.Instance.Equals(element2, element3); // false
```

```f#
let element1 = HtmlElement("span",
    HtmlAttribute("id", "foo"),
    HtmlText("bar"));

let element2 = HtmlElement("span",
    HtmlAttribute("id", "foo"),
    HtmlText("bar"));

let element3 = HtmlElement("span",
    HtmlAttribute("id", "foo"),
    HtmlText("oof"));

let firstTwoEqual = htmlEquals element1 element2 // true
let lastTwoEqual = htmlEquals element2 element3 // false
```

### Convert to Linq2Xml

You can convert LtGt's objects to `System.Xml.Linq` objects (`XNode`, `XElement`, etc). This can be useful if you need to convert HTML to XML or if you want to use XPath to select nodes.

```c#
var htmlDocument = Html.ParseDocument(html);
var xmlDocument = (XDocument) htmlDocument.ToXObject();
var elements = xmlDocument.XPathSelectElements("//input[@type=\"submit\"]");
```

```f#
let htmlDocument = Html.parseDocument html
let xmlDocument = htmlDocument |> toXObject :?> XDocument
let elements = xmlDocument.XPathSelectElements("//input[@type=\"submit\"]")
```

### Render nodes

You can turn any entity to its equivalent HTML code.

```c#
var element = new HtmlElement("div",
    new HtmlAttribute("id", "main"),
    new HtmlText("Hello world"));

var html = element.ToHtml(); // <div id="main">Hello world</div>
```

```f#
let element = HtmlElement("div",
    HtmlAttribute("id", "main"),
    HtmlText("Hello world"))

let html = element |> toHtml // <div id="main">Hello world</div>
```