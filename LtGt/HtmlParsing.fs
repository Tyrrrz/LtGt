namespace LtGt

open System
open FParsec
open LtGt.ParsingUtils

module private HtmlParsers =

    let upcastNode x = x :> HtmlNode

    // ---------------------------------
    // Text
    // ---------------------------------

    // * CDATA *
    // <![CDATA[content]]>
    let cdata =
        manyCharsBetween (skipString "<![CDATA[") anyChar (skipString "]]>") .>> spaces
        |>> HtmlText

    // * Normal text *
    let text =
        many1Chars (noneOf "<")
        |>> htmlDecode
        |>> String.trim
        |>> HtmlText

    // ---------------------------------
    // Comment
    // ---------------------------------

    // * Normal comment *
    // <!-- content -->
    let normalComment =
        manyCharsBetween (skipString "<!--") anyChar (skipString "-->") .>> spaces
        |>> String.trim
        |>> HtmlComment

    // * Unexpected XML directive *
    // -- treated as comment
    // <?xml version="1.0"?>
    let unexpectedDirectiveComment =
        manyCharsBetween (skipString "<?") anyChar (skipString "?>") .>> spaces
        |>> String.trim
        |>> HtmlComment

    // * Unexpected HTML declaration *
    // -- treated as comment
    // <!doctype html>
    let unexpectedDeclarationComment =
        manyCharsBetween (skipString "<!") anyChar (skipChar '>') .>> spaces
        |>> String.trim
        |>> HtmlComment

    let comment =
        choice [
            attempt normalComment
            attempt unexpectedDirectiveComment
            attempt unexpectedDeclarationComment
        ]

    // ---------------------------------
    // Attribute
    // ---------------------------------

    let attributeName = many1Satisfy (isNotSpace <&> isNoneOf ">'\"=/") .>> spaces

    // * Doubly-quoted attribute *
    // id="main"
    let doublyQuotedAttribute =
        attributeName .>> skipChar '=' .>> spaces .>>. manyCharsBetween (skipChar '"') anyChar (skipChar '"') .>> spaces
        |>> fun (name, value) -> (name, htmlDecode value)
        |>> HtmlAttribute

    // * Singly-quoted attribute *
    // id="main"
    let singlyQuotedAttribute =
        attributeName .>> skipChar '=' .>> spaces .>>. manyCharsBetween (skipChar ''') anyChar (skipChar ''') .>> spaces
        |>> fun (name, value) -> (name, htmlDecode value)
        |>> HtmlAttribute

    // * Unquoted attribute *
    // id=main
    let unquotedAttribute =
        attributeName .>> skipChar '=' .>> spaces .>>. attributeName .>> spaces
        |>> HtmlAttribute

    // * Void attribute *
    // id
    let voidAttribute =
        attributeName
        |>> HtmlAttribute

    let attribute =
        choice [
            attempt doublyQuotedAttribute
            attempt singlyQuotedAttribute
            attempt unquotedAttribute
            attempt voidAttribute
        ]

    // ---------------------------------
    // Element
    // ---------------------------------

    let elementName = many1Chars letterOrDigit .>> spaces

    // * Raw text element *
    // -- element that can only contain text inside
    // -- <script> and <style>

    let rawTextElementName = anyStringOfCI rawTextElementNames .>> spaces

    let rawTextElement =
        parse {
            // <script ...>
            do! skipChar '<'
            let! name = rawTextElementName
            let! attributes = many attribute
            do! skipChar '>'
            do! spaces

            // ...</script>
            let! text = (manyCharsTill anyChar (skipString (sprintf "</%s>" name))) |>> String.trim |>> HtmlText |>> upcastNode |>> Array.create 1
            do! spaces

            return HtmlElement(name, attributes, text)
        }

    // * Void element *
    // -- element that never has children and should not have a closing tag
    // -- <meta>, <br>, etc

    let voidElementName = anyStringOfCI voidElementNames .>> spaces

    let voidElement =
        skipChar '<' >>. voidElementName .>>. many attribute .>> spaces .>> optional (skipChar '/') .>> skipChar '>' .>> spaces
        |>> fun (name, attributes) -> (name, attributes, Array.empty)
        |>> HtmlElement

    // * Self-closing element *
    // -- element that doesn't have children and doesn't have a closing tag
    // -- this element is illegal in HTML but nobody really cares
    // -- <div />, <span />

    let selfClosingElement =
        skipChar '<' >>. elementName .>>. many attribute .>> spaces .>> skipString "/>" .>> spaces
        |>> fun (name, attributes) -> (name, attributes, Array.empty)
        |>> HtmlElement

    // * Normal element *
    // -- element that may have children and has a closing tag
    // -- <div>, <span>, etc

    // Element child parser is recursive as it can also contain other elements
    let elementChild, elementChildRef = createParserForwardedToRef<HtmlNode, unit>()

    let normalElement =
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

    let element =
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

    let declaration =
        manyCharsBetween (skipString "<!") anyChar (skipChar '>') .>> spaces
        |>> HtmlDeclaration

    let document =
        declaration .>>. many elementChild .>> spaces
        |>> HtmlDocument

    // ---------------------------------
    // Node
    // ---------------------------------

    let node =
        choice [
            attempt document |>> upcastNode
            attempt elementChild |>> upcastNode
        ]

module Html =

    let private runOrRaise parser source =
        match run parser source with
        | Success (r, _, _) -> r
        | Failure (e, _, _) -> raise (InvalidOperationException e)

    let public ParseDocument source = runOrRaise (spaces >>. HtmlParsers.document .>> eof) source

    let public ParseElement source = runOrRaise (spaces >>. HtmlParsers.element .>> eof) source

    let public ParseNode source = runOrRaise (spaces >>. HtmlParsers.node .>> eof) source