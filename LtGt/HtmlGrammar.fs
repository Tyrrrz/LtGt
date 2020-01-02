namespace LtGt

open FParsec
open LtGt.ParsingUtils

module private HtmlGrammar =

    let upcastNode x = x :> HtmlNode

    // ** Declaration

    // <!doctype html>
    let declaration =
        manyCharsBetween (skipString "<!") anyChar (skipChar '>') .>> spaces
        |>> HtmlDeclaration

    // ** Attribute

    let attributeName = many1Satisfy (isNotSpace <&> isNoneOf ">'\"=/") .>> spaces

    // Doubly-quoted attribute
    // id="main"
    let doublyQuotedAttribute =
        attributeName .>>? skipChar '=' .>>? spaces .>>.? manyCharsBetween (skipChar '"') anyChar (skipChar '"') .>> spaces
        |>> fun (name, value) -> (name, htmlDecode value)
        |>> HtmlAttribute

    // Singly-quoted attribute
    // id="main"
    let singlyQuotedAttribute =
        attributeName .>>? skipChar '=' .>>? spaces .>>.? manyCharsBetween (skipChar ''') anyChar (skipChar ''') .>> spaces
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
        manyCharsBetween (skipString "<!--") anyChar (skipString "-->") .>> spaces
        |>> String.trim
        |>> HtmlComment

    // Unexpected XML directive treated as comment
    // <?xml version="1.0"?>
    let unexpectedDirectiveComment =
        manyCharsBetween (skipString "<?") anyChar (skipString "?>") .>> spaces
        |>> String.trim
        |>> HtmlComment

    // Unexpected HTML declaration treated as comment
    // <!doctype html>
    let unexpectedDeclarationComment =
        manyCharsBetween (skipString "<!") anyChar (skipChar '>') .>> spaces
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
        manyCharsBetween (skipString "<![CDATA[") anyChar (skipString "]]>") .>> spaces
        |>> HtmlCData

    // ** Element

    let elementTagName = many1Chars <| choice [ letterOrDigit; pchar '-' ] .>> spaces

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
            let! children = manyCharsTill anyChar (skipString <| sprintf "</%s" tagName)
                            |>> String.trim
                            |>> HtmlText
                            |>> upcastNode
                            |>> Array.create 1
            do! spaces
            do! skipChar '>'
            do! spaces

            return HtmlElement(tagName, attributes, children)
        }

    // Void element
    // <meta name="foo" content="bar">
    let voidElement =
        skipChar '<' >>. voidElementTagName .>>. many attribute .>> spaces .>> (optional <| skipChar '/') .>> skipChar '>' .>> spaces
        |>> fun (tagName, attributes) -> (tagName, attributes, Array.empty)
        |>> HtmlElement

    // Self-closing element
    // <div />
    let selfClosingElement =
        skipChar '<' >>. elementTagName .>>. many attribute .>> spaces .>> skipString "/>" .>> spaces
        |>> fun (tagName, attributes) -> (tagName, attributes, Array.empty)
        |>> HtmlElement

    // Element child parser is recursive as it can also contain other elements
    let elementChild, elementChildRef = createParserForwardedToRef()

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

    let element =
        choice [
            attempt rawTextElement
            attempt voidElement
            attempt selfClosingElement
            attempt normalElement
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

/// Exception thrown when parsing fails.
//  Workaround: https://github.com/dotnet/fsharp/issues/3327#issuecomment-315025498
exception ParseException of message : string
    with override this.Message = this.message

// F# & C# API
module Html =

    let private fullDocument = spaces >>. HtmlGrammar.document .>> eof

    let private fullElement = spaces >>. HtmlGrammar.element .>> eof

    let private fullNode = spaces >>. HtmlGrammar.node .>> eof

    /// Tries to parse input string as an HTML document.
    [<CompiledName("TryParseDocument")>]
    let tryParseDocument source = runWithResult fullDocument source

    /// Tries to parse input string as an HTML element.
    [<CompiledName("TryParseElement")>]
    let tryParseElement source = runWithResult fullElement source

    /// Tries to parse input string as an HTML node.
    [<CompiledName("TryParseNode")>]
    let tryParseNode source = runWithResult fullNode source

    /// Parses input string as an HTML document or raises an exception in case of failure.
    [<CompiledName("ParseDocument")>]
    let parseDocument source =
        match tryParseDocument source with
        | Result.Ok res -> res
        | Result.Error err -> raise (ParseException err)

    /// Parses input string as an HTML element or raises an exception in case of failure.
    [<CompiledName("ParseElement")>]
    let parseElement source =
        match tryParseElement source with
        | Result.Ok res -> res
        | Result.Error err -> raise (ParseException err)

    /// Parses input string as an HTML node or raises an exception in case of failure.
    [<CompiledName("ParseNode")>]
    let parseNode source =
        match tryParseNode source with
        | Result.Ok res -> res
        | Result.Error err -> raise (ParseException err)