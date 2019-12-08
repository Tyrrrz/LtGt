module public LtGt.Parser.HtmlParser

open FParsec
open LtGt.Models
open LtGt.Parsers.Shared

// -- Helper functions

let private upcastNode x = x :> HtmlNode

// -- Text

// <![CDATA[content]]>
let private cdataText =
    manyCharsBetween anyChar (skipString "<![CDATA[") (skipString "]]>") .>> spaces
    |>> HtmlText

let private normalText =
    many1Chars (noneOf "<") .>> spaces
    |>> htmlDecode
    |>> trim
    |>> HtmlText
        
let private text =
    choice [
        attempt cdataText
        attempt normalText
    ]

// -- Comments

// <!-- content -->
let private normalComment =
    manyCharsBetween anyChar (skipString "<!--") (skipString "-->") .>> spaces
    |>> trim
    |>> HtmlComment

// Unexpected xml directives are treated as comments (Chrome's behavior)
// <?xml version="1.0"?>
let private unexpectedDirectiveComment =
    manyCharsBetween anyChar (skipString "<?") (skipString "?>") .>> spaces
    |>> trim
    |>> HtmlComment

// Unexpected html declarations are treated as comments (Chrome's behavior)
// <!doctype html>
let private unexpectedDeclarationComment =
    skipString "<!" >>. many1Chars letterOrDigit .>> spaces .>>. manyCharsTill anyChar (skipChar '>') .>> spaces
    |>> fun (name, value) -> name + value
    |>> trim
    |>> HtmlComment

let private comment =
    choice [
        attempt normalComment
        attempt unexpectedDirectiveComment
        attempt unexpectedDeclarationComment
    ]

// -- Attributes

let private attributeName = many1Satisfy (isNotSpace <&> isNoneOf ">'\"=/") .>> spaces

// id="main"
let private doublyQuotedAttribute =
    attributeName .>> skipChar '=' .>> spaces .>>. manyCharsBetween anyChar (skipChar '"') (skipChar '"') .>> spaces
    |>> fun (name, value) -> (name, htmlDecode value)
    |>> HtmlAttribute

// id='main'
let private singlyQuotedAttribute =
    attributeName .>> skipChar '=' .>> spaces .>>. manyCharsBetween anyChar (skipChar ''') (skipChar ''') .>> spaces
    |>> fun (name, value) -> (name, htmlDecode value)
    |>> HtmlAttribute

// id=main
let private unquotedAttribute =
    attributeName .>> skipChar '=' .>> spaces .>>. attributeName .>> spaces
    |>> HtmlAttribute

// id
let private valuelessAttribute =
    attributeName
    |>> HtmlAttribute

let private attribute =
    choice [
        attempt doublyQuotedAttribute
        attempt singlyQuotedAttribute
        attempt unquotedAttribute
        attempt valuelessAttribute
    ]

// -- Element

// Elements that can only contain text inside
// (ordered from most common to least common to avoid backtracking)
let private rawTextElementName =
    choice [
        attempt (pstringCI "script")
        attempt (pstringCI "style")
    ] .>> spaces

// Elements that should not have a closing tag
// (ordered from most common to least common to avoid backtracking)
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

let private normalElementName = many1Chars letterOrDigit .>> spaces

// Element child parser is recursive as it can also contain other elements
let private elementChild, private elementChildRef = createParserForwardedToRef<HtmlNode, unit>()

// <script> / <style>
let private rawTextElement =
    parse {
        // <script ...>
        do! skipChar '<'
        let! name = rawTextElementName
        let! attributes = many attribute
        do! spaces
        do! skipChar '>'
        do! spaces

        // ...</script>
        let! text = (manyCharsTill anyChar (skipString (sprintf "</%s>" name))) |>> trim |>> HtmlText
        do! spaces

        return name, attributes, text
    } |>> HtmlElement

// <meta> / <meta/> / <br> / <br/>
let private voidElement =
    skipChar '<' >>. voidElementName .>>. many attribute .>> spaces .>> optional (skipChar '/') .>> skipChar '>' .>> spaces
    |>> HtmlElement

// <div/>
let private selfClosingElement =
    skipChar '<' >>. normalElementName .>>. many attribute .>> spaces .>> skipString "/>" .>> spaces
    |>> HtmlElement

// <div>...</div>
let private normalElement =
    parse {
        // <div ...>
        do! skipChar '<'
        let! name = normalElementName
        let! attributes = many attribute
        do! skipChar '>'
        do! spaces

        // ...
        let! children = many elementChild

        // </div>
        do! skipString (sprintf "</%s>" name)
        do! spaces

        return name, attributes, children
    } |>> HtmlElement

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
        attempt comment |>> upcastNode
        attempt text |>> upcastNode
    ]

// -- Document
    
// <!doctype html>
let private declaration =
    skipString "<!" >>. many1Chars letterOrDigit .>> spaces .>>. manyCharsTill anyChar (skipChar '>') .>> spaces
    |>> HtmlDeclaration

let private document =
    declaration .>>. many elementChild .>> spaces
    |>> HtmlDocument

// -- Nodes

let private node =
    choice [
        attempt document |>> upcastNode
        attempt elementChild |>> upcastNode
    ]

// -- Entry points

let private runOrRaise parser source =
    match run parser source with
    | Success (r, _, _) -> r
    | Failure (e, _, _) -> raise (System.InvalidOperationException(e))

let public ParseElement source = runOrRaise element source

let public ParseDocument source = runOrRaise document source

let public ParseNode source = runOrRaise node source