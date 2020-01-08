namespace LtGt

open FParsec
open LtGt.ParsingUtils

module internal HtmlGrammar =

    let upcastNode x = x :> HtmlNode

    // ** Declaration

    // <!doctype html>
    let declaration =
        skipString "<!" |> anyStringBetween <| skipChar '>' .>> spaces
        |>> HtmlDeclaration

    // ** Attribute

    let attributeName = many1Satisfy (isNotSpace <&> isNoneOf ">'\"=/") .>> spaces

    // Doubly-quoted attribute
    // id="main"
    let doublyQuotedAttribute =
        attributeName .>>? skipChar '=' .>>? spaces .>>.? anyDoublyQuotedString .>> spaces
        |>> fun (name, value) -> (name, htmlDecode value)
        |>> HtmlAttribute

    // Singly-quoted attribute
    // id="main"
    let singlyQuotedAttribute =
        attributeName .>>? skipChar '=' .>>? spaces .>>.? anySinglyQuotedString .>> spaces
        |>> fun (name, value) -> (name, htmlDecode value)
        |>> HtmlAttribute

    // Unquoted attribute
    // id=main
    let unquotedAttribute =
        attributeName .>>? skipChar '=' .>>? spaces .>>. attributeName .>> spaces
        |>> HtmlAttribute

    // Empty attribute
    // id
    let emptyAttribute =
        attributeName
        |>> HtmlAttribute

    let attribute =
        choice [
            doublyQuotedAttribute
            singlyQuotedAttribute
            unquotedAttribute
            emptyAttribute
        ]

    // ** Text

    let text =
        many1Chars <| noneOf "<"
        |>> htmlDecode
        |>> String.trim
        |>> HtmlText

    // ** Comment

    // Normal comment
    // <!-- content -->
    let normalComment =
        skipString "<!--" |> anyStringBetween <| skipString "-->" .>> spaces
        |>> String.trim
        |>> HtmlComment

    // Unexpected XML directive treated as comment
    // <?xml version="1.0"?>
    let unexpectedDirectiveComment =
        skipString "<?" |> anyStringBetween <| skipString "?>" .>> spaces
        |>> String.trim
        |>> HtmlComment

    // Unexpected HTML declaration treated as comment
    // <!doctype html>
    let unexpectedDeclarationComment =
        skipString "<!" |> anyStringBetween <| skipChar '>' .>> spaces
        |>> String.trim
        |>> HtmlComment

    let comment =
        choice [
            normalComment
            unexpectedDirectiveComment
            unexpectedDeclarationComment
        ]

    // ** CData

    // <![CDATA[content]]>
    let cdata =
        skipString "<![CDATA[" |> anyStringBetween <| skipString "]]>" .>> spaces
        |>> HtmlCData

    // ** Element

    let elementTagName = many1Chars <| choice [ letter; digit; pchar '-' ] .>> spaces

    let rawTextElementTagName =
        rawTextElementTagNames
        |> List.map pstringCI
        |> choiceL <| "Raw text element tag"
        .>> spaces

    let voidElementTagName =
        voidElementTagNames
        |> List.map pstringCI
        |> choiceL <| "Void element tag"
        .>> spaces

    // Element child parser is recursive as it can also contain other elements
    let elementChild, elementChildRef = createParserForwardedToRef()

    // Raw text element
    // <script>foo = bar();</script>
    let rawTextElement =
        parse {
            // <script ...>
            do! skipChar '<'
            let! tagName = rawTextElementTagName
            let! attributes = many attribute
            do! skipChar '>'
            do! spaces

            // ...</script>
            let! children = anyChar |> manyCharsTill <| (skipString <| sprintf "</%s" tagName)
                            |>> String.trim
                            |>> HtmlText
                            |>> upcastNode
                            |>> List.singleton
            do! spaces
            do! skipChar '>'
            do! spaces

            return HtmlElement(tagName, attributes, children)
        }

    // Void element
    // <meta name="foo" content="bar">
    let voidElement =
        skipChar '<' >>. voidElementTagName .>>. many attribute .>> spaces .>> (optional <| skipChar '/') .>> skipChar '>' .>> spaces
        |>> fun (tagName, attributes) -> (tagName, attributes, List.empty)
        |>> HtmlElement

    // Self-closing element
    // <div />
    let selfClosingElement =
        skipChar '<' >>. elementTagName .>>. many attribute .>> spaces .>> skipString "/>" .>> spaces
        |>> fun (tagName, attributes) -> (tagName, attributes, List.empty)
        |>> HtmlElement

    // Normal element
    // <div><p>foo</p></div>
    let normalElement =
        parse {
            // <div ...>
            do! skipChar '<'
            let! tagName = elementTagName
            let! attributes = many attribute
            do! skipChar '>'
            do! spaces

            // ...
            let! children = many elementChild

            // </div>
            do! skipString <| sprintf "</%s" tagName
            do! spaces
            do! skipChar '>'
            do! spaces

            return HtmlElement(tagName, attributes, children)
        }

    // Unclosed element
    // <div><p>foo</p>
    let unclosedElement =
        parse {
            // <div ...>
            do! skipChar '<'
            let! tagName = elementTagName
            let! attributes = many attribute
            do! skipChar '>'
            do! spaces

            // ...
            let! children = many elementChild

            return HtmlElement(tagName, attributes, children)
        }

    let element =
        choice [
            attempt rawTextElement
            attempt voidElement
            attempt selfClosingElement
            attempt normalElement
            attempt unclosedElement
        ]

    do elementChildRef :=
        choice [
            element |>> upcastNode
            cdata |>> upcastNode
            comment |>> upcastNode
            text |>> upcastNode
        ]

    // ** Document

    let document =
        declaration .>>. many elementChild .>> spaces
        |>> HtmlDocument

    // ** Node

    let node =
        choice [
            document |>> upcastNode
            elementChild |>> upcastNode
        ]

    // ** Entry points

    let documentFull = spaces >>. document .>> eof

    let elementFull = spaces >>. element .>> eof

    let nodeFull = spaces >>. node .>> eof