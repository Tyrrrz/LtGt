# LtGt

[![Build](https://img.shields.io/appveyor/ci/Tyrrrz/LtGt/master.svg)](https://ci.appveyor.com/project/Tyrrrz/LtGt)
[![Tests](https://img.shields.io/appveyor/tests/Tyrrrz/LtGt/master.svg)](https://ci.appveyor.com/project/Tyrrrz/LtGt)
[![NuGet](https://img.shields.io/nuget/v/LtGt.svg)](https://nuget.org/packages/LtGt)
[![NuGet](https://img.shields.io/nuget/dt/LtGt.svg)](https://nuget.org/packages/LtGt)
[![Donate](https://img.shields.io/badge/patreon-donate-yellow.svg)](https://patreon.com/tyrrrz)
[![Donate](https://img.shields.io/badge/buymeacoffee-donate-yellow.svg)](https://buymeacoffee.com/tyrrrz)

LtGt is a minimalistic library for working with HTML. It can be used to parse HTML5-compliant code, traverse the resulting syntax tree, locate specific elements, and extract information. The library can also be used the other way around, to render HTML code from its document object model.

## Download

- [NuGet](https://nuget.org/packages/LtGt): `dotnet add package LtGt`
- [Continuous integration](https://ci.appveyor.com/project/Tyrrrz/LtGt)

## Features

- Parse and render HTML5-compliant code
- Traverse nodes using LINQ
- Find elements using methods like `GetElementById()`, `GetElementsByTagName()`, etc
- Find elements by evaluating CSS selectors using `GetElementsBySelector()`
- Convert the DOM to a Linq2Xml representation
- Easily extensible with custom workflows
- Targets .NET Framework 4.5+ and .NET Standard 1.0+

## Usage

### Parse a document

To parse an HTML document, you may create a new instance of `HtmlParser` or use a singleton `HtmlParser.Default`.

```c#
const string html = @"<!doctype html>
<html>
  <head>
    <title>Document</title>
  </head>
  <body>
    <div>Content</div>
  </body>
</html>";

var document = HtmlParser.Default.ParseDocument(html);
```

### Parse a fragment

Besides parsing a full document, you can also parse any other type of node.

```c#
const string html = "<div id=\"some-element\"><a href=\"https://example.com\">Link</a></div>";

// Parse an element node
var element = HtmlParser.Default.ParseElement(html);

// Parse any node
var node = HtmlParser.Default.ParseNode(html);
```

### Find specific element

There are many extension methods that should help you locate elements you want to find.

```c#
var element1 = document.GetElementById("menu-bar");
var element2 = document.GetElementByTagName("div");
var element3 = document.GetElementByClassName("floating-button floating-button--enabled");

var element1Data = element1.GetAttribute("data")?.Value;
var element2Id = element2.GetId();
var element2Text = element3.GetInnerText();
```

### Convert to Linq2Xml

It's possible to convert LtGt's objects to `System.Xml.Linq` objects (`XNode`, `XElement`, etc). This can be useful if you need to convert HTML to XML or if you want to use XPath to select nodes.

```c#
var htmlDocument = HtmlParser.Default.ParseDocument(html);

var xmlDocument = htmlDocument.ToXDocument();

var elements = xmlDocument.XPathSelectElements("//input[@type=\"submit\"]");
```

### Render nodes

You can convert any node or hierarchy of nodes to HTML code.

```c#
var element = new HtmlElement("div",
    new HtmlAttribute("id", "main"),
    new HtmlText("Hello world"));

var html = HtmlRenderer.Default.RenderNode(element); // <div id="main">Hello world</div>
```

## Libraries used

- [Sprache](https://github.com/Sprache/Sprache)
- [NUnit](https://github.com/nunit/nunit)

## Donate

If you really like my projects and want to support me, consider donating to me on [Patreon](https://patreon.com/tyrrrz) or [BuyMeACoffee](https://buymeacoffee.com/tyrrrz). All donations are optional and are greatly appreciated. 🙏