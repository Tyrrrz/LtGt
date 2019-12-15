module public LtGt.Parser.HtmlParser

open FParsec
open LtGt.Models
open LtGt.Parsers.Shared

let private upcastNode x = x :> HtmlNode

// ---------------------------------
// Text
// ---------------------------------

// * CDATA *
// <![CDATA[content]]>
let private cdata =
    manyCharsBetween (skipString "<![CDATA[") anyChar (skipString "]]>") .>> spaces
    |>> HtmlText

// * Normal text *
let private text =
    many1Chars (noneOf "<")
    |>> htmlDecode
    |>> trim
    |>> HtmlText
        
// ---------------------------------
// Comment
// ---------------------------------

// * Normal comment *
// <!-- content -->
let private normalComment =
    manyCharsBetween (skipString "<!--") anyChar (skipString "-->") .>> spaces
    |>> trim
    |>> HtmlComment

// * Unexpected XML directive *
// -- treated as comment
// <?xml version="1.0"?>
let private unexpectedDirectiveComment =
    manyCharsBetween (skipString "<?") anyChar (skipString "?>") .>> spaces
    |>> trim
    |>> HtmlComment

// * Unexpected HTML declaration *
// -- treated as comment
// <!doctype html>
let private unexpectedDeclarationComment =
    manyCharsBetween (skipString "<!") anyChar (skipChar '>') .>> spaces
    |>> trim
    |>> HtmlComment

let private comment =
    choice [
        attempt normalComment
        attempt unexpectedDirectiveComment
        attempt unexpectedDeclarationComment
    ]

// ---------------------------------
// Attribute
// ---------------------------------

let private attributeName = many1Satisfy (isNotSpace <&> isNoneOf ">'\"=/") .>> spaces

// * Doubly-quoted attribute *
// id="main"
let private doublyQuotedAttribute =
    attributeName .>> skipChar '=' .>> spaces .>>. manyCharsBetween (skipChar '"') anyChar (skipChar '"') .>> spaces
    |>> fun (name, value) -> (name, htmlDecode value)
    |>> HtmlAttribute

// * Singly-quoted attribute *
// id="main"
let private singlyQuotedAttribute =
    attributeName .>> skipChar '=' .>> spaces .>>. manyCharsBetween (skipChar ''') anyChar (skipChar ''') .>> spaces
    |>> fun (name, value) -> (name, htmlDecode value)
    |>> HtmlAttribute

// * Unquoted attribute *
// id=main
let private unquotedAttribute =
    attributeName .>> skipChar '=' .>> spaces .>>. attributeName .>> spaces
    |>> HtmlAttribute

// * Void attribute *
// id
let private voidAttribute =
    attributeName
    |>> HtmlAttribute

let private attribute =
    choice [
        attempt doublyQuotedAttribute
        attempt singlyQuotedAttribute
        attempt unquotedAttribute
        attempt voidAttribute
    ]

// ---------------------------------
// Element
// ---------------------------------

let private elementName = many1Chars letterOrDigit .>> spaces

// * Raw text element *
// -- element that can only contain text inside
// -- <script> and <style>

let private rawTextElementName =
    choice [
        attempt (pstringCI "script")
        attempt (pstringCI "style")
    ] .>> spaces

let private rawTextElement =
    parse {
        // <script ...>
        do! skipChar '<'
        let! name = rawTextElementName
        let! attributes = many attribute
        do! skipChar '>'
        do! spaces

        // ...</script>
        let! text = (manyCharsTill anyChar (skipString (sprintf "</%s>" name))) |>> trim |>> HtmlText
        do! spaces

        return HtmlElement(name, attributes, text)
    }

// * Void element *
// -- element that never has children and should not have a closing tag
// -- <meta>, <br>, etc

let private voidElementName =
    choice [
        attempt (pstringCI "meta")
        attempt (pstringCI "link")
        attempt (pstringCI "img")
        attempt (pstringCI "br")
        attempt (pstringCI "input")
        attempt (pstringCI "hr")
        attempt (pstringCI "area")
        attempt (pstringCI "base")
        attempt (pstringCI "col")
        attempt (pstringCI "embed")
        attempt (pstringCI "param")
        attempt (pstringCI "source")
        attempt (pstringCI "track")
        attempt (pstringCI "wbr")
    ] .>> spaces

let private voidElement =
    skipChar '<' >>. voidElementName .>>. many attribute .>> spaces .>> optional (skipChar '/') .>> skipChar '>' .>> spaces
    |>> HtmlElement

// * Self-closing element *
// -- element that doesn't have children and doesn't have a closing tag
// -- this element is illegal in HTML but nobody really cares
// -- <div />, <span />

let private selfClosingElement =
    skipChar '<' >>. elementName .>>. many attribute .>> spaces .>> skipString "/>" .>> spaces
    |>> HtmlElement

// * Normal element *
// -- element that may have children and has a closing tag
// -- <div>, <span>, etc

// Element child parser is recursive as it can also contain other elements
let private elementChild, private elementChildRef = createParserForwardedToRef<HtmlNode, unit>()

let private normalElement =
    parse {
        // <div ...>
        do! skipChar '<'
        let! name = elementName
        let! attributes = many attribute
        do! skipChar '>'
        do! spaces

        // ...
        let! children = many elementChild

        // </div>
        do! skipString (sprintf "</%s>" name)
        do! spaces

        return HtmlElement(name, attributes, children)
    }

let private element =
    choice [
        attempt rawTextElement
        attempt voidElement
        attempt selfClosingElement
        attempt normalElement
    ]

do elementChildRef :=
    choice [
        attempt element |>> upcastNode
        attempt cdata |>> upcastNode
        attempt comment |>> upcastNode
        attempt text |>> upcastNode
    ]

// ---------------------------------
// Document
// ---------------------------------

let private declaration =
    skipString "<!" >>. many1Chars letterOrDigit .>> spaces .>>. manyCharsTill anyChar (skipChar '>') .>> spaces
    |>> HtmlDeclaration

let private document =
    declaration .>>. many elementChild .>> spaces
    |>> HtmlDocument

// ---------------------------------
// Node
// ---------------------------------

let private node =
    choice [
        attempt document |>> upcastNode
        attempt elementChild |>> upcastNode
    ]

// ---------------------------------
// Entry points
// ---------------------------------

let private runOrRaise parser source =
    match run parser source with
    | Success (r, _, _) -> r
    | Failure (e, _, _) -> raise (System.InvalidOperationException(e))

let public ParseElement source = runOrRaise (element .>> eof) source

let public ParseDocument source = runOrRaise (document .>> eof) source

let public ParseNode source = runOrRaise (node .>> eof) source