namespace LtGt

open FParsec
open LtGt.ParsingUtils

module private CssSelectorGrammar =

    // Note to self:
    // Spaces are important in CSS selectors, don't skip them.

    // ** Attribute value operator

    let attributeValueOperator =
        choice [
            stringReturn "^=" StartsWith
            stringReturn "$=" EndsWith
            stringReturn "*=" Contains
            stringReturn "~=" WhiteSpaceSeparatedContains
            stringReturn "|=" HyphenSeparatedStartsWith
            charReturn '=' Equals
        ]

    // ** Number formula

    let even = stringReturn "even" <| MultiplierAndConstant (2, 0)

    let odd = stringReturn "odd" <| MultiplierAndConstant (2, 1)

    let formulaSign =
        choice [
            charReturn '-' -1
            charReturn '+' +1
            preturn +1
        ]

    let formulaNumber =
        many1Chars digit
        |>> int

    let formulaSignedNumber =
        formulaSign .>> spaces .>>. formulaNumber
        |>> fun (sign, number) -> sign * number

    let multiplierAndConstantFormula =
        formulaSignedNumber .>>? skipChar 'n' .>>. formulaSignedNumber
        |>> MultiplierAndConstant

    let onlyMultiplierNumberFormula =
        formulaSignedNumber .>>? skipChar 'n'
        |>> OnlyMultiplier

    let onlyConstantNumberFormula =
        formulaSignedNumber
        |>> OnlyConstant

    let numberFormula =
        choice [
            even
            odd
            multiplierAndConstantFormula
            onlyMultiplierNumberFormula
            onlyConstantNumberFormula
        ]

    // ** Selectors

    let exprChar =
        choice [
            skipChar '\\' >>? anyChar
            noneOf " .#:[]()>+~*^$|="
        ]

    let any = charReturn '*' Any

    let byTagName = many1Chars exprChar |>> ByTagName

    let byId = skipChar '#' >>. many1Chars exprChar |>> ById

    let byClassName = skipChar '.' >>. many1Chars exprChar |>> ByClassName

    let primitive =
        choice [
            any
            byTagName
            byId
            byClassName
        ]

    let byAttribute =
        skipChar '[' |> any1StringBetween <| skipChar ']'
        |>> ByAttribute

    let byDoublyQuotedAttributeValue =
        skipChar '[' >>. tuple3 (many1Chars exprChar) (attributeValueOperator) (anyDoublyQuotedString) .>> skipChar ']'
        |>> ByAttributeValue

    let bySinglyQuotedAttributeValue =
        skipChar '[' >>. tuple3 (many1Chars exprChar) (attributeValueOperator) (anySinglyQuotedString) .>> skipChar ']'
        |>> ByAttributeValue

    let attribute =
        choice [
            attempt byDoublyQuotedAttributeValue
            attempt bySinglyQuotedAttributeValue
            attempt byAttribute
        ]

    let root = stringCIReturn ":root" Root

    let empty = stringCIReturn ":empty" Empty

    let onlyChild = stringCIReturn ":only-child" OnlyChild

    let firstChild = stringCIReturn ":first-child" FirstChild

    let lastChild = stringCIReturn ":last-child" LastChild

    let nthChild =
        skipStringCI ":nth-child(" >>. numberFormula .>> skipChar ')'
        |>> NthChild

    let nthLastChild =
        skipStringCI ":nth-last-child(" >>. numberFormula .>> skipChar ')'
        |>> NthLastChild

    let onlyOfType = stringCIReturn ":only-of-type" OnlyOfType

    let firstOfType = stringCIReturn ":first-of-type" FirstOfType

    let lastOfType = stringCIReturn ":last-of-type" LastOfType

    let nthOfType =
        skipStringCI ":nth-of-type(" >>. numberFormula .>> skipChar ')'
        |>> NthOfType

    let nthLastOfType =
        skipStringCI ":nth-last-of-type(" >>. numberFormula .>> skipChar ')'
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
        skipString ":not(" >>. group .>> skipChar ')'
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

module internal CssSelector =

    let private selectorFull = CssSelectorGrammar.selector .>> eof

    let tryParse source = runWithResult selectorFull source