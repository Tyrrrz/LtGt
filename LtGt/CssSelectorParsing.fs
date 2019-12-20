namespace LtGt

open FParsec
open LtGt.ParsingUtils

module private CssSelectorParsers =

    // Note to self:
    // Don't skip trailing spaces in selectors because they are semantically important

    let exprChar =
        choice [
            attempt (skipChar '\\' >>. anyChar)
            attempt (noneOf " .#:[]()>+~*^$|=")
        ]

    // Primitive selectors

    let any = skipChar '*' >>% Any

    let byType = many1Chars letterOrDigit |>> ByType

    let byClass = skipChar '.' >>. many1Chars exprChar |>> ByClass

    let byId = skipChar '#' >>. many1Chars exprChar |>> ById

    let primitive =
        choice [
            any
            byType
            byClass
            byId
        ]

    // Attribute selectors

    let attributeValueEqualityOperator =
        choice [
            skipString "^=" >>% StartsWith
            skipString "$=" >>% EndsWith
            skipString "*=" >>% Contains
            skipString "~=" >>% WhiteSpaceSeparatedContains
            skipString "|=" >>% HyphenSeparatedStartsWith
            skipChar '=' >>% Equals
        ] .>> spaces

    let byAttribute =
        many1CharsBetween (skipChar '[') exprChar (skipChar ']')
        |>> ByAttribute

    let doublyQuotedByAttributeValue =
        skipChar '[' >>. tuple3 (many1Chars exprChar) attributeValueEqualityOperator (manyCharsBetween (skipChar '"') anyChar (skipChar '"')) .>> skipChar ']'
        |>> ByAttributeValue

    let singlyQuotedByAttributeValue =
        skipChar '[' >>. tuple3 (many1Chars exprChar) attributeValueEqualityOperator (manyCharsBetween (skipChar ''') anyChar (skipChar ''')) .>> skipChar ']'
        |>> ByAttributeValue

    let attribute =
        choice [
            attempt doublyQuotedByAttributeValue
            attempt singlyQuotedByAttributeValue
            attempt byAttribute
        ]

    // Hierarchical selectors

    let integerSign =
        choice [
            skipChar '-' >>% -1
            skipChar '+' >>% +1
            preturn +1
        ]

    let integer =
        integerSign .>> spaces .>>. many1Chars digit
        |>> fun (sign, str) -> sign * int str

    let even = skipString "even" >>% MultiplierAndConstant (2, 0)

    let odd = skipString "odd" >>% MultiplierAndConstant (2, 1)

    let fullNumberFormula = integer .>> skipChar 'n' .>>. integer |>> MultiplierAndConstant

    let onlyMultiplierNumberFormula = integer .>> skipChar 'n' |>> OnlyMultiplier

    let onlyConstantNumberFormula = integer |>> OnlyConstant

    let numberFormula =
        choice [
            even
            odd
            attempt fullNumberFormula
            attempt onlyMultiplierNumberFormula
            attempt onlyConstantNumberFormula
        ]

    let root = skipStringCI ":root" >>% Root

    let empty = skipStringCI ":empty" >>% Empty

    let onlyChild = skipStringCI ":only-child" >>% OnlyChild

    let firstChild = skipStringCI ":first-child" >>% FirstChild

    let lastChild = skipStringCI ":last-child" >>% LastChild

    let nthChild =
        skipStringCI ":nth-child(" >>. spaces >>. numberFormula .>> skipChar ')'
        |>> NthChild

    let nthLastChild =
        skipStringCI ":nth-last-child(" >>. spaces >>. numberFormula .>> skipChar ')'
        |>> NthLastChild

    let onlyOfType = skipStringCI ":only-of-type" >>% OnlyOfType

    let firstOfType = skipStringCI ":first-of-type" >>% FirstOfType

    let lastOfType = skipStringCI ":last-of-type" >>% LastOfType

    let nthOfType =
        skipStringCI ":nth-of-type(" >>. spaces >>. numberFormula .>> skipChar ')'
        |>> NthOfType

    let nthLastOfType =
        skipStringCI ":nth-last-of-type(" >>. spaces >>. numberFormula .>> skipChar ')'
        |>> NthLastOfType

    let hierarchical =
        choice [
            root
            empty
            onlyChild
            firstChild
            lastChild
            nthChild
            nthLastChild
            onlyOfType
            firstOfType
            lastOfType
            nthOfType
            nthLastOfType
        ]

    let standalone =
        choice [
            primitive
            attribute
            hierarchical
        ]

    let group, groupRef = createParserForwardedToRef()

    let not =
        skipString ":not(" >>. spaces >>. group .>> skipChar ')'
        |>> Not

    do groupRef :=
        choice [
            not
            standalone
        ]
        |> many1
        |>> Group

    let descendant =
        group .>> spaces1 .>>. group
        |>> Descendant

    let child =
        group .>> spaces .>> skipChar '>' .>> spaces .>>. group
        |>> Child

    let sibling =
        group .>> spaces .>> skipChar '+' .>> spaces .>>. group
        |>> Sibling

    let subsequentSibling =
        group .>> spaces .>> skipChar '~' .>> spaces .>>. group
        |>> SubsequentSibling

    let standaloneCombinator =
        choice [
            attempt descendant
            attempt child
            attempt sibling
            attempt subsequentSibling
            attempt group
        ]

    let recursiveDescendant =
        standaloneCombinator .>> spaces1 .>>. sepBy1 group spaces1
        |>> fun (seed, others) -> (others |> Seq.fold (fun acc x -> Descendant (acc, x)) seed)

    let recursiveChild =
        standaloneCombinator .>> spaces .>> skipChar '>' .>> spaces .>>. sepBy1 group (spaces .>> skipChar '>' .>> spaces)
        |>> fun (seed, others) -> (others |> Seq.fold (fun acc x -> Child (acc, x)) seed)

    let recursiveSibling =
        standaloneCombinator .>> spaces .>> skipChar '+' .>> spaces .>>. sepBy1 group (spaces .>> skipChar '+' .>> spaces)
        |>> fun (seed, others) -> (others |> Seq.fold (fun acc x -> Sibling (acc, x)) seed)

    let recursiveSubsequentSibling =
        standaloneCombinator .>> spaces .>> skipChar '~' .>> spaces .>>. sepBy1 group (spaces .>> skipChar '~' .>> spaces)
        |>> fun (seed, others) -> (others |> Seq.fold (fun acc x -> SubsequentSibling (acc, x)) seed)

    let recursiveCombinator =
        choice [
            attempt recursiveDescendant
            attempt recursiveChild
            attempt recursiveSibling
            attempt recursiveSubsequentSibling
        ]

    let selector =
        choice [
            attempt recursiveCombinator
            attempt standaloneCombinator
        ]

// F# API (not accessible from C#)
module internal CssSelector =

    /// Parses input string as a CSS selector or raises an exception in case of failure.
    let public parse source = runOrRaise (CssSelectorParsers.selector .>> eof) source