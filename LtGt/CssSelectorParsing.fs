namespace LtGt

open FParsec

module private CssSelectorParsers =
    let exprChar : Parser<char, unit> =
        choice [
            noneOf " .#:[]()>+~*^$|="
            skipChar '\\' >>. anyChar
        ]

    let attributeValueEqualityOperator =
        choice [
            skipString "^=" >>% StartsWith
            skipString "$=" >>% EndsWith
            skipString "*=" >>% Contains
            skipString "~=" >>% WhiteSpaceSeparatedContains
            skipString "|=" >>% HyphenSeparatedStartsWith
            skipChar '=' >>% Equals
        ] .>> spaces

    let integerSign : Parser<int, unit> =
        choice [
            skipChar '-' >>% -1
            skipChar '+' >>% +1
            preturn +1
        ] .>> spaces

    let integer =
        integerSign .>>. manyChars digit .>> spaces
        |>> fun (sign, str) -> sign * int str


    // * Full formula *
    // 2n + 1, n - 5, etc
    let fullNumberFormula =
        integer .>> skipChar 'n' .>>. integer
        |>> MultiplierAndConstant

    // * Formula without constant *
    // 2n, n, etc
    let onlyMultiplierNumberFormula =
        integer .>> skipChar 'n' .>> spaces
        |>> OnlyMultiplier

    // * Formula without multiplier *
    // 2, 5, 7
    let onlyConstantNumberFormula =
        integer
        |>> OnlyConstant

    let numberFormula =
        choice [
            attempt fullNumberFormula
            attempt onlyMultiplierNumberFormula
            attempt onlyConstantNumberFormula
        ]

    let anySelector : Parser<Selector, unit> =
        skipChar '*' >>% Any .>> spaces

    let typeSelector =
        many1Chars letterOrDigit |>> ByType .>> spaces

    let classSelector =
        skipChar '.' >>. many1Chars exprChar |>> ByClass .>> spaces

    let idSelector =
        skipChar '#' >>. many1Chars exprChar |>> ById .>> spaces

    // * Attribute selectors *

    let byAttributeSelector =
        many1CharsBetween (skipChar '[') exprChar (skipChar ']') .>> spaces
        |>> ByAttribute

    let doublyQuotedByAttributeValueSelector =
        skipChar '[' >>. tuple3 (many1Chars exprChar) attributeValueEqualityOperator (manyCharsBetween (skipChar '"') anyChar (skipChar '"')) .>> skipChar ']' .>> spaces
        |>> ByAttributeValue

    let singlyQuotedByAttributeValueSelector =
        skipChar '[' >>. tuple3 (many1Chars exprChar) attributeValueEqualityOperator (manyCharsBetween (skipChar ''') anyChar (skipChar ''')) .>> skipChar ']' .>> spaces
        |>> ByAttributeValue

    let attributeSelector =
        choice [
            attempt byAttributeSelector
            attempt doublyQuotedByAttributeValueSelector
            attempt singlyQuotedByAttributeValueSelector
        ]

module internal CssSelector =
    let public Parse source = Any // temp