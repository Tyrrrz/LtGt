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
- Convenient methods for working with DOM
- Extensible object model
- Targets .NET Framework 4.5+ and .NET Standard 1.0+

## Usage

### Parse a document

To parse an HTML document, you may create a new instance of `HtmlParser` or use a singleton `HtmlParser.Default`.

```c#
const string html = @"<doctype html>
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

Besides parsing a full document, you can also parse any other node.

```c#
const string html = "<div id=\"some-element\"><a href=\"https://example.com\">Link</a></div>";

var node = HtmlParser.Default.ParseNode(html);

var element = (HtmlElement) node; // we assume we're dealing with an element
```

### Find specific element

There are many extension methods that should help work with DOM more easily.

```c#
var elem1 = document.GetElementById("menu-bar");
var elem2 = document.GetElementByTagName("div");
var elem3 = document.GetElementByClassName("floating-button floating-button--enabled");

var elem1Data = elem1.GetAttribute("data")?.Value;
var elem2Id = elem2.GetId();
var elem2Text = elem3.GetInnerText();
```

### Render nodes

You can convert any node or hierarchy of nodes to HTML code.

```c#
var element = new HtmlElement("div",
    new HtmlAttribute("id", "main"),
    new HtmlText("Hello world"));

var html = HtmlRenderer.Default.RenderNode(element);
```

## Libraries used

- [Sprache](https://github.com/Sprache/Sprache)
- [NUnit](https://github.com/nunit/nunit)
- [Tyrrrz.Extensions](https://github.com/Tyrrrz/Extensions)

## Donate

If you really like my projects and want to support me, consider donating to me on [Patreon](https://patreon.com/tyrrrz) or [BuyMeACoffee](https://buymeacoffee.com/tyrrrz). All donations are optional and are greatly appreciated. 🙏